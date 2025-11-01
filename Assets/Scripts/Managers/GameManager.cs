using BoardAdventures.Abstractions;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private IGameFlowService _gameFlowService;
        private ITurnVisualizer _turnVisualizer;


        [Inject]
        public void Initialize(IGameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            // start game
           _gameFlowService.StartGame();
        }

        private void EndGame()
        {
            // end game
        }
    }
}