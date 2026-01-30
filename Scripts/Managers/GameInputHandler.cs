using BoardAdventures.Abstractions;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Managers
{
    public class GameInputHandler: MonoBehaviour, IGameInputHandler
    {
        private SignalBus _signalBus;

        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void HandleDiceRollRequest()
        {
            _signalBus.Fire(new OnDiceRollRequestedSignal());
        }
    }
}