using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.UI.Players;
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
        [SerializeField] private Button playButton;
        
        private SignalBus _signalBus;


        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<OnPlayerListUpdatedSignal>(HandlePlayerListInLobby);
            _signalBus.Subscribe<OnPlayerListUpdatedSignal>(HandlePlayButton);

            HideAllPlayerSlotViews();
            ShowWaitingToJoinPlayer(true);
            playButton.interactable = false;
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

        private void HandlePlayButton(OnPlayerListUpdatedSignal signal)
        {
            playButton.interactable = signal.Players.Count >= signal.MaxPlayer;
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
    }
}