using System;
using System.Collections.Generic;
using BoardAdventures.Abstractions;
using UnityEngine;
using Zenject;

namespace BoardAdventures.UI.Players
{
    public class PlayerUIFactory : MonoBehaviour, IPlayerUIFactory
    {
        [SerializeField] private PlayerUI playerUIPrefab;
        [SerializeField] private RectTransform playerUIParent;
        private DiContainer _container;

        [Inject]
        public void Initialize(DiContainer container)
        {
            _container = container;
            Debug.Log("DiContainer successfully injected into PlayerUIFactory.");
        }

        public PlayerUI Create(string playerName, List<Color> factionColors)
        {
            var ui = _container.InstantiatePrefabForComponent<PlayerUI>(
                playerUIPrefab, playerUIParent);
            
            ui.SetPlayerUI(playerName,factionColors);
            
            return ui;
        }
    }
}