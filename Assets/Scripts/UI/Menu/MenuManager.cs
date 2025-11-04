using System;
using System.Text;
using BoardAdventures.Abstractions;
using Core.Data;
using UnityEngine;
using Zenject;

namespace BoardAdventures.UI.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private CanvasGroup mainPanel;

        private CanvasGroup _lastPanel;
        private SignalBus _signalBus;


        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _lastPanel = mainPanel;
        }

        public void ShowPanel(CanvasGroup panel)
        {
            if (_lastPanel != null)
            {
                _lastPanel.alpha = 0;
                _lastPanel.interactable = false;
                _lastPanel.blocksRaycasts = false;
            }

            _lastPanel = panel;

            panel.alpha = 1;
            panel.interactable = true;
            panel.blocksRaycasts = true;
        }

        public void OnGameModeClicked(GameMode mode)
        {
            
        }
    }
}