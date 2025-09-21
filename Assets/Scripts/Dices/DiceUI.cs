using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dices
{
    public class DiceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepTxt;
        [SerializeField] private Button rollButton;
        [SerializeField] private CanvasGroup canvas;
        public void RollDice(int step)
        {
            stepTxt.text = step.ToString();
        }

        public void SetActivateDice(bool isActive)
        {
            canvas.alpha = isActive ? 1f : 0.4f;
            rollButton.interactable = isActive;
        }

        public void Reset()
        {
            stepTxt.text = "--";
        }
        
    }
}