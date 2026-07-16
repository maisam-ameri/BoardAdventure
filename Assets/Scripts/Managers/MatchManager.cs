using BoardAdventures.Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Managers
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private float turnDuration = 10;
        private SignalBus _signalBus;
        
        [Inject]
        private void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Start()
        {
            _signalBus.Fire(new OnStartMatchSignal{TurnDuration = turnDuration});
        }
    }
}