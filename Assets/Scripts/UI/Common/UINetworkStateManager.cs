using BoardAdventures.Network;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BoardAdventures.UI.Common
{
    public class UINetworkStateManager : MonoBehaviour
    {
        [SerializeField] private GameObject waitingToConnect;
        [SerializeField] private Button matchButton;
        private SignalBus _signalBus;

        [Inject]
        private void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;

            _signalBus.Subscribe<OnConnectionStatusChangedSignal>(HandleUIConnectingToServer);
        }

        private void HandleUIConnectingToServer(OnConnectionStatusChangedSignal signal)
        {
            Debug.Log(signal.State);
            switch (signal.State)
            {
                case ConnectionState.Connecting:
                    waitingToConnect.SetActive(true);
                    matchButton.interactable = false;
                    break;
                
                case ConnectionState.ConnectedToMaster:
                    waitingToConnect.SetActive(false);
                    matchButton.interactable = true;
                    break;
                
                case ConnectionState.Disconnected:
                    waitingToConnect.SetActive(true);
                    matchButton.interactable = false;
                    break;
            }
        }
    }
}