using BoardAdventures.Authentication;
using BoardAdventures.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Presentation.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainPanel;
        [SerializeField] private CanvasGroup matchPanel;
        [SerializeField] private CanvasGroup lobbyPanel;
        [SerializeField] private CanvasGroup registrationPanel;

        private CanvasGroup _lastPanel;
        private SignalBus _signalBus;
        private IAccountService _accountService;


        [Inject]
        public void Initialize(SignalBus signalBus, IAccountService accountService)
        {
            _signalBus = signalBus;
            _accountService = accountService;
        }

        private void Start()
        {
            _signalBus.Subscribe<OnShowRegistrationUISignal>(HandleShowRegistrationUI);
            _signalBus.Subscribe<OnPlayerLoggedInSignal>(HandleLoggedInUI);

            HideAllPanels();
            _accountService.CheckAuth();
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
            HandleHidePanel(registrationPanel);
        }

        private void HandleShowRegistrationUI()
        {
            ShowPanel(registrationPanel);
        }

        private void HandleLoggedInUI()
        {
            _signalBus.Fire(new OnConnectionRequestSignal());

            ShowPanel(mainPanel);
        }

        public void OnSelectMatchClicked(int maxPlayer)
        {
            _signalBus.Fire(new OnJoinToRoomRequestSignal {MaxPlayers = (byte) maxPlayer});

            ShowPanel(lobbyPanel);
        }

        public void OnRegisterClicked(TMP_InputField inputField)
        {
            _signalBus.Fire(new OnRegisterRequestedSignal {Nickname = inputField.text});
        }
    }
}