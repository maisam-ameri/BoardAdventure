using BoardAdventures.Abstractions;
using BoardAdventures.UI.Menu;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private IGameFlowService _gameFlowService;
        private ITurnVisualizer _turnVisualizer;
        private IMenuManager _menuManager;

        [Inject]
        public void Initialize(IGameFlowService gameFlowService, IMenuManager menuManager)
        {
            _gameFlowService = gameFlowService;
            _menuManager = menuManager;
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