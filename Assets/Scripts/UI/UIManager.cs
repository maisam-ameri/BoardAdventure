using BoardAdventures.UI.Players;
using UnityEngine;

namespace BoardAdventures.UI
{
    public class UIManager: MonoBehaviour
    {
        [SerializeField] private PlayerUI playerUIPrefab;
        [SerializeField] private RectTransform playerUIParent;

        public PlayerUI PlayerUIPrefab => playerUIPrefab;
        public RectTransform PlayerUIParent => playerUIParent;
    }
}