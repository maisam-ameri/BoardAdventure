using System;
using System.Collections.Generic;
using BoardAdventures.Abstractions;
using UnityEngine;

namespace BoardAdventures.UI.Players
{
    public class PlayerUIFactory : IPlayerUIFactory
    {
        private readonly PlayerUI _playerUIPrefab;
        private readonly RectTransform _playerUIParent;

        public PlayerUIFactory(PlayerUI playerUIPrefab, RectTransform playerUIParent)
        {
            _playerUIPrefab = playerUIPrefab;
            _playerUIParent = playerUIParent;
        }

        public PlayerUI Create(string playerName, List<Color> factionColors,
            Action onTurnTimerExpired)
        {
            var ui = UnityEngine.Object.Instantiate(_playerUIPrefab, _playerUIParent, true);
            ui.SetPlayerUI(playerName,factionColors,onTurnTimerExpired);
            return ui;
        }
    }
}