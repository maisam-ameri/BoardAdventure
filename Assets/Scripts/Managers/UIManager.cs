using Players.UI;
using UnityEngine;

namespace Managers
{
    public class UIManager: MonoBehaviour
    {
        [SerializeField] private PlayerUI playerUIPrefab;
        [SerializeField] private RectTransform playerUIParent;
        public PlayerUI CreatePlayerUI()
        {
            var ui =Instantiate(playerUIPrefab, playerUIParent, true);
            return ui;
        }
    }
}