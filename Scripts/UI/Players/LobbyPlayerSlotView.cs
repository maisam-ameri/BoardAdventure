using TMPro;
using UnityEngine;

namespace BoardAdventures.UI.Players
{
    public class LobbyPlayerSlotView: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nickName;

        public void SetData(string nickname)
        {
            nickName.text = nickname;
        }
    }
}