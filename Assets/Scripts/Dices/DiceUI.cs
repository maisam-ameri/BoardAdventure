using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dices
{
    public class DiceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepTxt;
        [SerializeField] private Button rollButton;
        public void RollDice(int step)
        {
            stepTxt.text = step.ToString();
        }

        public void ActivateRoll(bool isActive)
        {
            rollButton.enabled = isActive;
        }
        
    }
}