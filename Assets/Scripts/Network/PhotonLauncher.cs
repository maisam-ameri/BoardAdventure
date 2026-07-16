using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.Authentication;
using BoardAdventures.Config;
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
        public Player LocalPlayer => PhotonNetwork.LocalPlayer;

        private SignalBus _signalBus;
        private IAccountService _accountService;
        private INetworkConfigProvider _networkConfigProvider;


        [Inject]
        private void Initialize(SignalBus signalBus, IAccountService accountService,
            INetworkConfigProvider networkConfigProvider)
        {
            _signalBus = signalBus;
            _accountService = accountService;
            _networkConfigProvider = networkConfigProvider;
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public T GetRoomProp<T>(string key, T defaultValue = default)
        {
            return NetworkHelper.GetCustomProperty<T>(key);
        }

        public T GetPlayerProp<T>(string key, Player player = null, T defaultValue = default)
        {
            player ??= PhotonNetwork.LocalPlayer;

            return NetworkHelper.GetPlayerCustomProperty<T>(key, player);
        }

        public void SetPlayerCustomProperty<T>(string key, T prop = default)
        {
            NetworkHelper.SetPlayerCustomProperty(key, prop);
        }

        public void SetCustomProperty<T>(string key, T prop = default)
        {
            NetworkHelper.SetCustomProperty(key, prop);
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

            var settings = _networkConfigProvider.GetConfig();
            PhotonNetwork.ConnectUsingSettings(settings);

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

        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            if (changedProps.ContainsKey(NetworkKeys.DiceRollRequestedKey))
                _signalBus.Fire(new OnDiceRollRequestedNetSignal());

            if (changedProps.ContainsKey(NetworkKeys.StepKey))
                _signalBus.Fire(new OnDiceRolledSignal {Step = (int?) changedProps[NetworkKeys.StepKey]});
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(NetworkKeys.ReadyToPlayKey))
                _signalBus.Fire(new OnLobbyStateChangedSignal());

            if (changedProps.ContainsKey(NetworkKeys.TurnEndTimeKey))
                _signalBus.Fire(new OnTurnEndTimeChangedSignal());
        }

        public bool CheckAllPlayersReady()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player.IsMasterClient) continue;

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