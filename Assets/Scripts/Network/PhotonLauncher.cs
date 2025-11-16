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

        [Inject]
        private void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                OnConnectedToMaster();
                return;
            }

            PhotonNetwork.ConnectUsingSettings();
            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.Connecting});
        }

        public override void OnConnectedToMaster()
        {
            _signalBus.Fire(new OnConnectionStatusChangedSignal {State = ConnectionState.ConnectedToMaster});
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