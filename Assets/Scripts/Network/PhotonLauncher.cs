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
        public byte MaxPlayers => PhotonNetwork.CurrentRoom.MaxPlayers;
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;

        private SignalBus _signalBus;
        private IAccountService _accountService;
        

        [Inject]
        private void Initialize(SignalBus signalBus, IAccountService accountService)
        {
            _signalBus = signalBus;
            _accountService = accountService;
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public T GetPlayerProp<T>(string key,Player player = null, T defaultValue = default)
        {
            player ??= PhotonNetwork.LocalPlayer;

            return NetworkHelper.GetPlayerCustomProperty<T>(key, player);
        }

        public void SetPlayerReady<T>(string key, T prop = default)
        {
            NetworkHelper.SetPlayerCustomProperty(key,prop);
        }

        public void Connect()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("[NetworkManager] Internet restored! Ready to connect.");
                _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.ConnectionFailed});

                return;
            }

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
            _signalBus.Fire(new OnLobbyStateChangedSignal());
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.NickName = _accountService.Nickname;
            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.ConnectedToMaster});
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _signalBus.Fire(new OnLobbyStateChangedSignal());
        }

        public override void OnLeftRoom()
        {
            _signalBus.Fire(new OnLobbyStateChangedSignal());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _signalBus.Fire(new OnLobbyStateChangedSignal());
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log(changedProps.ContainsKey(NetworkKeys.ReadyToPlayKey));
            if (changedProps.ContainsKey(NetworkKeys.ReadyToPlayKey))
                _signalBus.Fire(new OnLobbyStateChangedSignal());
        }

        public bool CheckAllPlayersReady()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if(player.IsMasterClient) continue;
                
                var value = GetPlayerProp<bool>(NetworkKeys.ReadyToPlayKey, player);

                if (value is false)
                    return false;
            }

            return true;
        }

        public void LoadLevel(string levelName)
        {
            PhotonNetwork.LoadLevel(levelName);
        }

        public List<Player> GetPlayers()
        {
            if (!PhotonNetwork.InRoom) return null;

            return PhotonNetwork.CurrentRoom.Players
                .OrderBy(p => p.Value.ActorNumber)
                .Select(p => p.Value)
                .ToList();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (cause == DisconnectCause.DnsExceptionOnConnect
                || cause == DisconnectCause.ExceptionOnConnect
                || cause == DisconnectCause.ClientTimeout
                || cause == DisconnectCause.DisconnectByClientLogic
                || cause == DisconnectCause.ServerTimeout)
            {
                Debug.Log(cause);

                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    Debug.Log("[NetworkManager] Internet restored! Ready to connect.");
                    _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.ConnectionFailed});

                    return;
                }

                PhotonNetwork.Reconnect();
            }

            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.Disconnected});
        }
    }
}