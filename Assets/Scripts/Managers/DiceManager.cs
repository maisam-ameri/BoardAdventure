using System;
using BoardAdventures.Core.Dices;
using BoardAdventures.UI.Dices;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private DiceUI diceUI;
        private Dice _dice;

        public event Action<int?> OnDiceRolled;
        public int? Step { get; private set; }


        private void Start()
        {
            _dice = new Dice();
        }

        public void RollDice()
        {
            Step = _dice.Roll();
            diceUI.ShowRoll(Step.Value);
            OnDiceRolled?.Invoke(Step);
        }

        // for debugging
        public void RollDice(int step)
        {
            Step = step;
            diceUI.ShowRoll(Step.Value);
            OnDiceRolled?.Invoke(Step);
        }

        public void SetActivateDice(bool isActive)
        {
            diceUI.SetInteractable(isActive);
        }

        public void Reset()
        {
            diceUI.Reset();
            Step = null;
        }
    }
}