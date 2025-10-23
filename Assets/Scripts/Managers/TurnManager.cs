using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.GameLogic;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.UI.Common;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class TurnManager : MonoBehaviour
    {
        private DiceManager _diceManager;
        private PawnManager _pawnManager;
        private TurnVisualizer _turnVisualizer;
        private IMovement _mover;
        private List<Player> _players;
        private int _currentPlayerIndex;
        private bool _firstSix;
        private bool _isDiceRolled;
        private GameFlowService _gameFlowService;
        private PlayerSetupService _playerSetupService;
        private Player CurrentPlayer => _gameFlowService.CurrentPlayer;


        private void Start()
        {
            InitializeManagers();

            _players = _playerSetupService.Players;
            _turnVisualizer.Initial(_players);
            _turnVisualizer.DeactivateTurnVisuals();
            _turnVisualizer.DeactivatePlayerVisuals();
            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, null);
        }

        private void InitializeManagers()
        {
            GameServices.Initialize();
            _diceManager = GameServices.DiceManager;
            _pawnManager = GameServices.PawnManager;
            _turnVisualizer = GameServices.TurnVisualizer;
            _gameFlowService = GameServices.GameFlowService;
            _playerSetupService = GameServices.PlayerSetupService;
            _mover = GameServices.Mover;

            _playerSetupService.OnTurnTimerExpired += OnTurnTimerExpired;
            _mover.OnCaptured += HandleCapturePawn;
            _gameFlowService.OnRewardGranted += HandleRewardGranted;

            _pawnManager.Initialize(new PawnFactory(), new PawnStateService());
            _playerSetupService.Initialize();
            _gameFlowService.InitializePlayers(_playerSetupService.Players);
        }

        private void HandleCapturePawn(IPawn pawn)
        {
            _pawnManager.ReturnPawnToBase(pawn);
        }


        private void HandleRewardGranted(Player player)
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