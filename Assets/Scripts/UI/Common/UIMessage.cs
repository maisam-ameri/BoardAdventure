using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardAdventures.UI.Common
{
    public class UIMessage: MonoBehaviour
    {
        [SerializeField] private Image panel;
        [SerializeField] private TextMeshProUGUI messageTxt;

        public void ShowMessage(string message, float duration = 0.5f)
        {
            StartCoroutine(ShowMessageCoroutine(message, duration));
        }

        private IEnumerator ShowMessageCoroutine(string msg, float duration)
        {
            var showTime = new WaitForSeconds(duration);
            messageTxt.text = msg;
            
            yield return showTime;
            Destroy(panel.gameObject);
            
        }
    }
}