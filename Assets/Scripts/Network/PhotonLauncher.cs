using System.Collections.Generic;
using BoardAdventures.Abstractions;
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
        private void Initialize(SignalBus signalBus,IAccountService accountService)
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
        
        public void JoinToRoom()
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public override void OnJoinedRoom()
        {
            _signalBus.Fire(new OnPlayerListUpdatedSignal {Players = PhotonNetwork.CurrentRoom.Players});
        }


        public override void OnConnectedToMaster()
        {
            PhotonNetwork.NickName = _accountService.Nickname;
            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.ConnectedToMaster});
        }
        

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _signalBus.Fire(new OnPlayerListUpdatedSignal {Players = PhotonNetwork.CurrentRoom.Players});
        }

        public Dictionary<int, Player> GetPlayers()
        {
            return PhotonNetwork.CurrentRoom.Players;
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
    }
}