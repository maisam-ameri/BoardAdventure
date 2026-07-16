using System.Collections.Generic;
using BoardAdventures.Network;
using BoardAdventures.Presentation.Menu;
using BoardAdventures.Presentation.Players;
using BoardAdventures.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BoardAdventures.Presentation.Lobby
{
    public class LobbyManager : MonoBehaviour, ILobbyManager
    {
        [SerializeField] private List<LobbyPlayerSlotView> playerSlots;
        [SerializeField] private TextMeshProUGUI waitingToJoin;
        [SerializeField] private TextMeshProUGUI roomStatus;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button startButton;
        [SerializeField] private string levelName;

        private SignalBus _signalBus;
        private ILobbyService _lobbyService;


        [Inject]
        public void Initialize(SignalBus signalBus, INetworkService networkService,
            ILobbyService lobbyService)
        {
            _signalBus = signalBus;
            _lobbyService = lobbyService;
        }

        private void Start()
        {
            _signalBus.Subscribe<OnLobbyStateUiChangedSignal>(HandleLobbyStateChanged);

            DeactivateAllButtons();
            HideAllPlayerSlotViews();
            ShowWaitingToJoinPlayer(true);
        }

        public void ReadyToPlayClicked()
        {
            _lobbyService.ToggleReady();
        }

        public void StartMatchClicked()
        {
            _lobbyService.StartMatch(levelName);
        }

        private void HideAllPlayerSlotViews()
        {
            foreach (var slotView in playerSlots)
            {
                slotView.gameObject.SetActive(false);
            }
        }

        private void ShowWaitingToJoinPlayer(bool isActive)
        {
            waitingToJoin.gameObject.SetActive(isActive);
        }

        private void HandleLobbyStateChanged(OnLobbyStateUiChangedSignal signal)
        {
            UpdateButtons(signal);
            UpdatePlayerList();
            UpdateLobbyMessages(signal);
        }


        private void UpdatePlayerList()
        {
            var players = _lobbyService.Players;

            HideAllPlayerSlotViews();
            ShowWaitingToJoinPlayer(players.Count == 0);
            
            for (var i = 0; i < players.Count; i++)
            {
                UpdatePlayerSlots(players[i], playerSlots[i]);
            }
        }

        private void UpdatePlayerSlots(Photon.Realtime.Player player, LobbyPlayerSlotView playerSlot)
        {
                var nickname = player.IsMasterClient ? $"{player.NickName} (Master)" : player.NickName;
                var isReady = player.IsMasterClient || _lobbyService.IsPlayerReady(player);

                playerSlot.gameObject.SetActive(true);
                playerSlot.SetPlayerSlot(nickname, isReady);
        }

        private void DeactivateAllButtons()
        {
            startButton.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(false);
        }

        private void UpdateButtons(OnLobbyStateUiChangedSignal signal)
        {
            readyButton.gameObject.SetActive(!signal.IsMaster);
            startButton.gameObject.SetActive(signal.IsMaster);
            startButton.interactable = signal.State == LobbyState.ReadyToStart;
        }

        private void UpdateLobbyMessages(OnLobbyStateUiChangedSignal signal)
        {
            var msg = "";
            roomStatus.text = msg;

            switch (signal.State)
            {
                case LobbyState.WaitingForPlayers:
                    msg = "Waiting for players to join";
                    break;

                case LobbyState.WaitingForReady:
                    msg = "Waiting for players to be ready";
                    break;

                case LobbyState.WaitingForHost:
                    msg = "Waiting for host to start";
                    break;

                case LobbyState.ReadyToStart:
                    msg = "Ready to start";
                    break;
            }

            roomStatus.text = msg;
        }
    }
}