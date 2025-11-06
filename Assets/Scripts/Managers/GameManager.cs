using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using Signals;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        private SignalBus _signalBus;
        

        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void StartGame(int playerCount)
        {
            _signalBus.Fire(new OnGameStartSignal {PlayerCount = playerCount});
        }

        private void EndGame(Player winner)
        {
            
            _signalBus.Fire(new OnGameOverSignal{Winner = winner});
        }
    }
}