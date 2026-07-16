using BoardAdventures.Board;
using BoardAdventures.Network;
using BoardAdventures.Presentation.Dices;
using BoardAdventures.Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Managers
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private DiceUI diceUI;
        private Dice _dice;
        private SignalBus _signalBus;
        private INetworkService _networkService;

        public int? Step { get; private set; }
        public bool IsRolled { get; set; }


        [Inject]
        public void Initialize(SignalBus signalBus, INetworkService networkService)
        {
            _signalBus = signalBus;
            _networkService = networkService;
        }

        private void Start()
        {
            _dice = new Dice();
            _signalBus.Subscribe<OnDiceRollRequestedNetSignal>(RollDice);
        }

        public void RollDice()
        {
            if (!_networkService.IsMasterClient) return;
            
            Step = _dice.Roll();
            _networkService.SetCustomProperty(NetworkKeys.StepKey, Step);

            // for debug
            //if (Step == 6) Step = 1;
        }

        public void UpdateDiceUI(int step)
        {
            diceUI.Roll(step);
        }

        // for debugging
        public void RollDice(int step)
        {
            Step = step;
            diceUI.Roll(Step.Value);

            // if (Step == 6)
            //     _signalBus.Fire(new OnFirstSixRolledSignal());

            _signalBus.Fire(new OnDiceRolledSignal {Step = Step});
        }

        public void SetActivateDice(bool isActive)
        {
            diceUI.SetInteractable(isActive);
        }

        public void Reset()
        {
            diceUI.Reset();
            Step = null;
            IsRolled = false;
        }
    }
}