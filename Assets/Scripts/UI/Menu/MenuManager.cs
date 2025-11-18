using BoardAdventures.Abstractions;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.UI.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private CanvasGroup mainPanel;
        [SerializeField] private CanvasGroup matchPanel;
        [SerializeField] private CanvasGroup lobbyPanel;

        private CanvasGroup _lastPanel;
        private SignalBus _signalBus;
        private INetworkService _networkService;

        [Inject]
        public void Initialize(SignalBus signalBus, INetworkService networkService)
        {
            _signalBus = signalBus;
            _networkService = networkService;
        }

        private void Start()
        {
            HideAllPanels();
            ShowPanel(mainPanel);
            _lastPanel = mainPanel;
            _networkService.Connect();
            // _signalBus.Subscribe<OnGameOverSignal>(HandleGameResultPanel);
        }



        public void ShowPanel(CanvasGroup panel)
        {
            if (_lastPanel != null)
                HandleHidePanel(_lastPanel);

            _lastPanel = panel;
            HandleShowPanel(panel);
        }

        private void HandleShowPanel(CanvasGroup panel)
        {
            panel.alpha = 1;
            panel.interactable = true;
            panel.blocksRaycasts = true;
        }

        private void HandleHidePanel(CanvasGroup panel)
        {
            panel.alpha = 0;
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }

        private void HideAllPanels()
        {
            HandleHidePanel(mainPanel);
            HandleHidePanel(matchPanel);
            HandleHidePanel(lobbyPanel);
            // HandleHidePanel(matchResultPanel);
        }

        // private void HandleGameResultPanel(OnGameOverSignal signal)
        // {
        //     ShowPanel(matchResultPanel);
        //     winnerName.text = $"{signal.Winner.Name} won";
        // }

        public void OnSelectMatchClicked(int playerCount)
        {
            // HideAllPanels();
            _networkService.JoinToRoom();
            ShowPanel(lobbyPanel);
            _signalBus.Fire(new OnPlayerListUpdatedSignal());
        }
    }
}