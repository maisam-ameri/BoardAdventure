using BoardAdventures.Abstractions;
using BoardAdventures.UI.Menu;
using Core.Data;
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

        public void StartGame(GameMode mode)
        {
            _signalBus.Fire(new OnGameStartSignal{Mode = mode});
        }

        private void EndGame()
        {
        }
    }
}