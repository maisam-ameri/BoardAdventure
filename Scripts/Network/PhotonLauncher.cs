using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Network
{
    public class PhotonLauncher : MonoBehaviourPunCallbacks, INetworkService
    {
        private SignalBus _signalBus;
        private IAccountService _accountService;

        [Inject]
        private void Initialize(SignalBus signalBus, IAccountService accountService)
        {
            _signalBus = signalBus;
            _accountService = accountService;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                OnConnectedToMaster();
                return;
            }

            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
            PhotonNetwork.ConnectUsingSettings();

            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.Connecting});
        }

        public void JoinToRoom(byte maxPlayer)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions {MaxPlayers = maxPlayer});
        }


        public override void OnJoinedRoom()
        {
            _signalBus.Fire(new OnPlayerListUpdatedSignal
            {
                MaxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers,
                Players = PhotonNetwork.CurrentRoom.Players
            });
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.NickName = _accountService.Nickname;
            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.ConnectedToMaster});
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _signalBus.Fire(new OnPlayerListUpdatedSignal
            {
                MaxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers,
                Players = PhotonNetwork.CurrentRoom.Players
            });
        }

        public override void OnLeftRoom()
        {
            _signalBus.Fire(new OnPlayerListUpdatedSignal
            {
                MaxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers,
                Players = PhotonNetwork.CurrentRoom.Players
            });
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _signalBus.Fire(new OnPlayerListUpdatedSignal
            {
                MaxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers,
                Players = PhotonNetwork.CurrentRoom.Players
            });
        }

        private bool CheckPlayersReadyToPlay()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player.CustomProperties.TryGetValue("IsReadyToPlay", out var value))

                    if (value is bool isReady && !isReady)
                        return false;
            }

            return true;
        }

        public void SetPlayerReady(bool isReady)
        {
            Hashtable props = new() {{"IsReadyToPlay", isReady}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }


        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey("IsReadyToPlay"))
                if (CheckPlayersReadyToPlay())
                    _signalBus.Fire(new OnPlayersReadyToPlaySignal());
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (cause == DisconnectCause.DnsExceptionOnConnect
                || cause == DisconnectCause.ExceptionOnConnect
                || cause == DisconnectCause.ClientTimeout
                || cause == DisconnectCause.ServerTimeout)
            {
                Debug.Log(cause);
                PhotonNetwork.Reconnect();
            }

            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.Disconnected});
        }

        public List<Core.Players.Player> GetPlayers()
        {
            return PhotonNetwork.CurrentRoom.Players
                .OrderBy(p => p.Value.ActorNumber)
                .Select(p => p.Value)
                .Select(p => new Core.Players.Player{Nickname = p.NickName})
                .ToList();
        }
    }
}
