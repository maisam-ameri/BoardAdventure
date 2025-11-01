using System;
using BoardAdventures.Abstractions;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private IGameFlowService _gameFlowService;
        private ITurnVisualizer _turnVisualizer;
        private SignalBus _signalBus;
        public Action OnStartGame { get; set; }
        public Action OnEndGame { get; set; }


        [Inject]
        public void Initialize(IGameFlowService gameFlowService, SignalBus signalBus)
        {
            _gameFlowService = gameFlowService;
            _signalBus = signalBus;
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            // start game
           _gameFlowService.StartGame();
            OnStartGame?.Invoke();
        }

        private void EndGame()
        {
            // end game
            OnEndGame?.Invoke();
        }
    }
}