using BoardAdventures.Abstractions;
using Signals;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class MatchManager : MonoBehaviour, IMatchManager
    {
        private SignalBus _signalBus;
        [Inject]
        private void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Start()
        {
            _signalBus.Fire(new OnStartMatchSignal());
        }
    }
}