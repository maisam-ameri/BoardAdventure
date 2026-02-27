using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.Network;
using BoardAdventures.UI.Players;
using Signals;
using TMPro;
using UI.Menu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Player = BoardAdventures.Core.Players.Player;

namespace BoardAdventures.UI.Lobby
{
    public class LobbyManager : MonoBehaviour, ILobbyManager
    {
        [SerializeField] private List<LobbyPlayerSlotView> playerUIList;
        [SerializeField] private TextMeshProUGUI waitingToJoin;
        [SerializeField] private TextMeshProUGUI roomStatus;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button startButton;
        [SerializeField] private string matchLevelName;

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
            _signalBus.Subscribe<OnPlayerListUpdatedSignal>(HandlePlayerListUpdated);
            _signalBus.Subscribe<OnAllPlayersReadySignal>(HandleAllPlayersReady);
            
            DeactivateAllButtons();
            HideAllPlayerSlotViews();
            ShowWaitingToJoinPlayer(true);
        }

        public void ReadyToPlayClicked()
        {
            _networkService.SetPlayerReady(true);
        }

        public void StartMatchClicked()
        {
            if (!_networkService.IsMasterClient)
                return;

            _networkService.LoadLevel(matchLevelName);
        }

        private void HideAllPlayerSlotViews()
        {
            foreach (var slotView in playerUIList)
            {
                slotView.gameObject.SetActive(false);
            }
        }

        private void ShowWaitingToJoinPlayer(bool isActive)
        {
            waitingToJoin.gameObject.SetActive(isActive);
        }

        private void HandleAllPlayersReady()
        {
            startButton.interactable = true;
        }

        private void HandlePlayerListUpdated(OnPlayerListUpdatedSignal signal)
        {
            ShowWaitingToJoinPlayer(false);

            for (var i = 0; i < signal.Players.Count; i++)
            {
                var playerData = signal.Players.ElementAt(i).Value;
                playerUIList[i].gameObject.SetActive(true);

                var nickname = playerData.NickName;

                var isReady = false;

                if (playerData.CustomProperties.TryGetValue(NetworkKeys.ReadyToPlayKey, out var value))
                {
                    if (value is bool ready)
                        isReady = ready;
                }

                UpdateLobbyUI(_networkService.GetPlayers(), _networkService.MaxPlayers);
                playerUIList[i].SetPlayerSlot(nickname, isReady);
            }
        }

        private LobbyState EvaluateLobbyState(List<Player> players, byte maxPlayer)
        {
            if (players.Count < maxPlayer)
                return LobbyState.WaitingForPlayers;

            if (!_networkService.CheckAllPlayersReady())
                return LobbyState.WaitingForReady;

            if (!_networkService.IsMasterClient)
                return LobbyState.WaitingForHost;

            return LobbyState.ReadyToStart;
        }

        private void DeactivateAllButtons()
        {
            roomStatus.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(false);
            readyButton.interactable = false;
            startButton.gameObject.SetActive(false);
            startButton.interactable = false;
        }
        
        private void UpdateLobbyUI(List<Player> players, byte maxPlayer)
        {
            var state = EvaluateLobbyState(players, maxPlayer);
            roomStatus.gameObject.SetActive(true);

            UpdateButtons(state);
            UpdateLobbyMessages(state);
        }

        private void UpdateButtons(LobbyState state)
        {
            var isMaster = _networkService.IsMasterClient;
            var isLocalReady = false;

            if (_networkService.GetPlayerCustomProperties()
                .TryGetValue(NetworkKeys.ReadyToPlayKey, out var value))
            {
                if (value is bool ready)
                    isLocalReady = ready;
            }

            readyButton.gameObject.SetActive(true);
            readyButton.interactable = !isLocalReady;

            switch (state)
            {
                case LobbyState.WaitingForPlayers:
                    startButton.gameObject.SetActive(false);
                    break;

                case LobbyState.WaitingForReady:
                    startButton.gameObject.SetActive(isMaster);
                    startButton.interactable = false;
                    break;

                case LobbyState.WaitingForHost:
                    startButton.gameObject.SetActive(false);
                    break;

                case LobbyState.ReadyToStart:
                    startButton.gameObject.SetActive(isMaster);
                    startButton.interactable = true;
                    break;
            }
        }
        
        private void UpdateLobbyMessages(LobbyState state)
        {
            var msg = "";
            switch (state)
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