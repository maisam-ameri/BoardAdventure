using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.UI.Players;
using Photon.Pun;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BoardAdventures.UI.Lobby
{
    public class LobbyManager : MonoBehaviour, ILobbyManager
    {
        [SerializeField] private List<LobbyPlayerSlotView> playerUIList;
        [SerializeField] private TextMeshProUGUI waitingToJoin;
        [SerializeField] private Button readyButton;
        
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
            _signalBus.Subscribe<OnPlayerListUpdatedSignal>(HandlePlayerListInLobby);
            _signalBus.Subscribe<OnPlayerListUpdatedSignal>(HandleReadyButton);
            _signalBus.Subscribe<OnPlayersReadyToPlaySignal>(HandlePlayersReadyToPlay);

            HideAllPlayerSlotViews();
            ShowWaitingToJoinPlayer(true);
            readyButton.interactable = false;
        }

        public void ReadyToPlayClicked()
        {
            _networkService.SetPlayerReady(true);
        }

        private void HandlePlayerListInLobby(OnPlayerListUpdatedSignal signal)
        {
            HideAllPlayerSlotViews();
            ShowWaitingToJoinPlayer(false);

            for (var i = 0; i < signal.Players.Count; i++)
            {
                playerUIList[i].gameObject.SetActive(true);
                var nickname = signal.Players.ElementAt(i).Value.NickName;
                playerUIList[i].SetData(nickname);
            }
        }

        private void HandleReadyButton(OnPlayerListUpdatedSignal signal)
        {
            readyButton.interactable = signal.Players.Count >= signal.MaxPlayer;
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

        private void HandlePlayersReadyToPlay()
        {
            // todo: handle UI
            PhotonNetwork.LoadLevel("Match");
        }
    }
}