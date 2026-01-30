using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardAdventures.UI.Dices
{
    public class DiceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepTxt;
        [SerializeField] private Button rollButton;
        [SerializeField] private CanvasGroup canvas;
        public void ShowRoll(int step)
        {
            stepTxt.text = step.ToString();
        }

        public void SetInteractable(bool isActive)
        {
            canvas.alpha = isActive ? 1f : 0.4f;
            rollButton.interactable = isActive;
        }

        public void Reset()
        {
            stepTxt.text = "Roll";
        }
        
    }
}