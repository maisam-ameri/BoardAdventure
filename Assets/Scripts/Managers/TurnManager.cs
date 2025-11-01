using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.GameLogic;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.UI.Common;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Managers
{
    public class TurnManager: ITurnManager
    {
        private IDiceManager _diceManager;
        private IPawnManager _pawnManager;
        private ITurnVisualizer _turnVisualizer;
        private List<Player> _players;
        private int _currentPlayerIndex;
        private bool _firstSix;
        private bool _isDiceRolled;
        private IGameFlowService _gameFlowService;
        private IPlayerSetupService _playerSetupService;
        private Player CurrentPlayer => _gameFlowService.CurrentPlayer;
        private SignalBus _signalBus;

        // private void Start()
        // {
        //     //InitializeManagers();
        //
        //     _players = _playerSetupService.Players;
        //     _turnVisualizer.Initial(_players);
        //     _turnVisualizer.DeactivateTurnVisuals();
        //     _turnVisualizer.DeactivatePlayerVisuals();
        //     _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, null);
        // }

        [Inject]
        public void Initialize(IGameFlowService gameFlowService
            , IDiceManager diceManager, IPawnManager pawnManager
            , ITurnVisualizer turnVisualizer, IPlayerSetupService playerSetupService
            , SignalBus signalBus)
        {
            _signalBus = signalBus;
            _gameFlowService = gameFlowService;
            _diceManager = diceManager;
            _pawnManager = pawnManager;
            _turnVisualizer = turnVisualizer;
            _playerSetupService = playerSetupService;

            _signalBus.Subscribe<OnRewardGrantedSignal>(HandleRewardGranted);
            _signalBus.Subscribe<OnTurnTimerExpiredSignal>(OnTurnTimerExpired);
            _signalBus.Subscribe<OnCapturedSignal>(HandleCapturePawn);

            // _pawnManager.Initialize(new PawnFactory(), new PawnStateService());
            // _playerSetupService.Initialize();
            // _gameFlowService.InitializePlayers(_playerSetupService.Players);
        }


        private void HandleCapturePawn(OnCapturedSignal signal)
        {
            _pawnManager.ReturnPawnToBase(signal.Pawn);
        }


        private void HandleRewardGranted()
        {
            _diceManager.SetActivateDice(true);
        }

        private void SwitchTurn()
        {
            _gameFlowService.SwitchTurn();
        }

        private void OnTurnTimerExpired()
        {
            SwitchTurn();
        }
    }
}