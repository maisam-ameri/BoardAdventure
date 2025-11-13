using BoardAdventures.Abstractions;
using Photon.Pun;
using Photon.Realtime;
using Signals;
using Zenject;

namespace Network
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
            _signalBus.Fire(new OnConnectingToServer());
        }

        public override void OnConnectedToMaster()
        {
            _signalBus.Fire(new OnConnectedToServer());
        }
    }
}