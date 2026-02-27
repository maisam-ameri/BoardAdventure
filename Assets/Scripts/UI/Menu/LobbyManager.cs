using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.Network;
using BoardAdventures.UI.Players;
using Photon.Pun;
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
            if (!PhotonNetwork.IsMasterClient)
                return;

            PhotonNetwork.LoadLevel("Match");
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

                var isMaster = playerData.IsMasterClient ? " (Master)" : "";
                var nickname = playerData.NickName + isMaster;


                var isReady = false;

                if (playerData.CustomProperties.TryGetValue(NetworkKeys.ReadyToPlayKey, out var value))
                {
                    if (value is bool ready)
                        isReady = ready;
                }

                UpdateLobbyUI(_networkService.GetPlayers(), PhotonNetwork.CurrentRoom.MaxPlayers);
                playerUIList[i].SetPlayerSlot(nickname, isReady);
            }
        }

        private LobbyState EvaluateLobbyState(List<Player> players, byte maxPlayer)
        {
            if (players.Count < maxPlayer)
                return LobbyState.WaitingForPlayers;

            if (!_networkService.CheckAllPlayersReady())
                return LobbyState.WaitingForReady;

            if (!PhotonNetwork.IsMasterClient)
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
            var isMaster = PhotonNetwork.IsMasterClient;
            var isLocalReady = false;

            if (PhotonNetwork.LocalPlayer.CustomProperties
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
            switch (state)
            {
                case LobbyState.WaitingForPlayers:
                    roomStatus.text = "Waiting for players to join";
                    startButton.interactable = false;
                    break;

                case LobbyState.WaitingForReady:
                    roomStatus.text = "Waiting for players to be ready";
                    startButton.interactable = false;
                    break;

                case LobbyState.WaitingForHost:
                    roomStatus.text = "Waiting for host to start";
                    startButton.interactable = false;
                    break;

                case LobbyState.ReadyToStart:
                    roomStatus.text = "Ready to start";
                    startButton.interactable = true;
                    break;
            }
        }
    }
}