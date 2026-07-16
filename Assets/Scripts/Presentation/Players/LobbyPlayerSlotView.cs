using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardAdventures.UI.Players
{
    public class LobbyPlayerSlotView: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nickName;
        [SerializeField] private Image readyIcon;

        public void SetPlayerSlot(string nickname, bool isReady)
        {
            nickName.text = nickname;
            readyIcon.color = isReady ? Color.green : Color.red;
        }
    }
}