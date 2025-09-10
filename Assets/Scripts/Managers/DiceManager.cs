using System;
using Dices;
using UnityEngine;

namespace Managers
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
            diceUI.RollDice(Step.Value);
            OnDiceRolled?.Invoke(Step);
        }

        public void SetActivateDice(bool isActive)
        {
            diceUI.SetActivateDice(isActive);
        }
    }
}