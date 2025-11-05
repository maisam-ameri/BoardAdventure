using BoardAdventures.Abstractions;
using UnityEngine;
using Zenject;

namespace BoardAdventures.UI.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private CanvasGroup mainPanel;
        [SerializeField] private CanvasGroup matchPanel;

        private CanvasGroup _lastPanel;
        private IGameManager _gameManager;

        [Inject]
        public void Initialize(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Start()
        {
            _lastPanel = mainPanel;
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
        }

        public void OnStartMatchClicked(int playerCount)
        {
            HideAllPanels();
            _gameManager.StartGame(playerCount);
        }
    }
}