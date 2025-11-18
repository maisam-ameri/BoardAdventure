using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace BoardAdventures.UI.Players
{
    public class LobbyPlayerSlotView: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nickName;

        public void SetData(Player player)
        {
            nickName.text = $"{player.ActorNumber} - {player.NickName}";
        }
    }
}