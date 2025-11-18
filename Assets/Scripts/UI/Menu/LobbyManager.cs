using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.UI.Players;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.UI.Lobby
{
    public class LobbyManager: MonoBehaviour, ILobbyManager
    {
        [SerializeField] private List<LobbyPlayerSlotView> playerUIList;
        private SignalBus _signalBus;

        [Inject]
        public void Initialize(SignalBus signalBus, INetworkService networkService)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<OnPlayerListUpdatedSignal>(HandlePlayerListInLobby);
        }
        
        private void HandlePlayerListInLobby(OnPlayerListUpdatedSignal signal)
        {
            for (int i = 0; i < playerUIList.Count; i++)
            {
                if(i>= signal.Players.Count) return;

                playerUIList[i].gameObject.SetActive(true);
                playerUIList[i].SetData(signal.Players[i]);
            }
        }
        
    }
}