using System;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Dices;
using BoardAdventures.UI.Dices;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Managers
{
    public class DiceManager : MonoBehaviour, IDiceManager
    {
        [SerializeField] private DiceUI diceUI;
        private Dice _dice;
        private SignalBus _signalBus;

        public int? Step { get; private set; }
        public bool IsRolled { get; set; }


        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _dice = new Dice();
        }

        public void RollDice()
        {
            Step = _dice.Roll();
            diceUI.ShowRoll(Step.Value);

            // if (Step == 6)
            //     _signalBus.Fire(new OnFirstSixRolledSignal());

            _signalBus.Fire(new OnDiceRolledSignal {Step = Step});
        }

        // for debugging
        public void RollDice(int step)
        {
            Step = step;
            diceUI.ShowRoll(Step.Value);

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