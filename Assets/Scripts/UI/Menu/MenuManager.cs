using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace BoardAdventures.UI.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private CanvasGroup mainPanel;
        [SerializeField] private CanvasGroup matchPanel;
        [SerializeField] private CanvasGroup matchResultPanel;
        [SerializeField] private TextMeshProUGUI winnerName;

        private CanvasGroup _lastPanel;
        private SignalBus _signalBus;

        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            HideAllPanels();
            ShowPanel(mainPanel);
            _lastPanel = mainPanel;

            _signalBus.Subscribe<OnGameOverSignal>(HandleGameResultPanel);
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
            HandleHidePanel(matchResultPanel);
        }

        private void HandleGameResultPanel(OnGameOverSignal signal)
        {
            ShowPanel(matchResultPanel);
            winnerName.text = $"{signal.Winner.Name} won";
        }

        public void OnStartMatchClicked(int playerCount)
        {
            HideAllPanels();
            _signalBus.Fire(new OnGameStartSignal{PlayerCount = playerCount});
        }
    }
}