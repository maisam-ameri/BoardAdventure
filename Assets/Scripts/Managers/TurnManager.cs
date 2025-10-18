using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private UIMessageManager _uiMessageManager;
        private TurnVisualizer _turnVisualizer;
        private IMovement _mover;
        private List<Player> _players;
        private int _currentPlayerIndex;
        private bool _firstSix;
        private bool _isDiceRolled;
        private TurnLogicService _turnLogicService;
        private PlayerActionValidator _playerActionValidator;
        private GameFlowService _gameFlowService;
        private PawnMovementService _movementService;
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
            _uiMessageManager = GameServices.UIMessageManager;
            _playerActionValidator = GameServices.PlayerActionValidator;
            _turnLogicService = GameServices.TurnLogicService;
            _gameFlowService = GameServices.GameFlowService;
            _movementService = GameServices.PawnMovementService;
            _playerSetupService = GameServices.PlayerSetupService;
            _mover = GameServices.Mover;

            _playerSetupService.OnTurnTimerExpired += OnTurnTimerExpired;
            _playerSetupService.OnSelectedPawn += OnSelectedPawn;
            _diceManager.OnDiceRolled += OnDiceRolled;
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

        private void OnSelectedPawn(IPawn pawn)
        {
            if (!_diceManager.IsRolled)
            {
                _uiMessageManager.ShowRollMessage();
                return;
            }

            var faction = pawn.Faction;

            if (pawn.State == "InBase" && _diceManager.Step == 6)
            {
                if (CurrentPlayer.Factions.All(f => f != faction)) return;

                if (!faction.StartNode.IsEmpty)
                {
                    _uiMessageManager.ShowStartNodeMessage();
                    return;
                }

                _pawnManager.EnterPawnToGame(pawn, OnActionCompleted);
            }
            else if (pawn.State == "InGame")
            {
                _ = HandleSelectedPawnAsync(pawn);
            }
        }

        private async Task HandleSelectedPawnAsync(IPawn pawn)
        {
            await _movementService.MovePawn(CurrentPlayer, pawn, _diceManager.Step, OnActionCompleted);
        }

        private void OnActionCompleted()
        {
            //_isDiceRolled = false;
            _diceManager.Reset();

            if (_turnLogicService.HasReward)
            {
                _gameFlowService.GrantReward(CurrentPlayer);
            }
            else
            {
                SwitchTurn();
            }
        }

        private void OnDiceRolled(int? step)
        {
            if (step is null) return;

            _diceManager.IsRolled = true;
            _diceManager.SetActivateDice(false);
            CurrentPlayer.UI.StopTimer();
            CurrentPlayer.UI.StartTurnTimer(10);


            var canEnter = _playerActionValidator.CheckToEnterPawn(CurrentPlayer, step);
            var canMove = _playerActionValidator.CheckToMovePawn(CurrentPlayer, step);

            var decision = _turnLogicService.ProcessRoll(step, canEnter, canMove);

            switch (decision)
            {
                case TurnDecision.WaitForAction:
                    _uiMessageManager.ShowActionAvailableMessage(CurrentPlayer.Name, step.Value);
                    CurrentPlayer.UI.StartTurnTimer(10);
                    break;
                case TurnDecision.RollReward:
                    _uiMessageManager.ShowRewardMessage(CurrentPlayer.Name);
                    _diceManager.SetActivateDice(true);
                    break;

                case TurnDecision.SwitchTurn:
                    SwitchTurn();
                    break;
            }
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