using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.GameLogic;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.UI;
using BoardAdventures.UI.Common;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class TurnManager : MonoBehaviour
    {
        private DiceManager _diceManager;
        private PawnManager _pawnManager;
        private UIManager _uiManager;
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

        public Action OnTurnSwitched { get; set; }

        public Player CurrentPlayer
        {
            get => _players[_currentPlayerIndex];
            set { }
        }

        private Player _lastPlayer;


        private void Start()
        {
            InitializeManagers();
            var factions = InitializeFactions();
            _players = CreatePlayer(factions);
            InitialPlayerUIs();
            CurrentPlayer = _players[0];
            _turnVisualizer.Initial(_players);
            _turnVisualizer.DeactivateTurnVisuals();
            _turnVisualizer.DeactivatePlayerVisuals();
            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, _lastPlayer);
        }

        private void InitializeManagers()
        {
            GameServices.Initialize();
            _diceManager = GameServices.DiceManager;
            _pawnManager = GameServices.PawnManager;
            _uiManager = GameServices.UIManager;
            _turnVisualizer = GameServices.TurnVisualizer;
            _uiMessageManager = GameServices.UIMessageManager;
            _playerActionValidator = GameServices.PlayerActionValidator;
            _turnLogicService = GameServices.TurnLogicService;
            _gameFlowService = GameServices.GameFlowService;
            _movementService = GameServices.PawnMovementService;
            _mover = GameServices.Mover;

            _pawnManager.Initialize(new PawnFactory(),new PawnStateService());
            _diceManager.OnDiceRolled += OnDiceRolled;
            _mover.OnCaptured += HandleCapturePawn;
            _gameFlowService.OnTurnStarted += HandleTurnStarted;
            _gameFlowService.OnTurnEnded += HandleTurnEnded;
            _gameFlowService.OnRewardGranted += HandleRewardGranted;
        }

        private void HandleCapturePawn(IPawn pawn)
        {
            _pawnManager.ReturnPawnToBase(pawn);
        }


        private void HandleTurnStarted(Player player)
        {
            _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, _lastPlayer);
            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, _lastPlayer);
            player.UI.StartTurnTimer(10);
        }

        private void HandleTurnEnded(Player player)
        {
            player.UI.StopTimer();
        }

        private void HandleRewardGranted(Player player)
        {
            _diceManager.SetActivateDice(true);
        }


        private List<Player> CreatePlayer(List<Faction> factions)
        {
            var factionPlayer1 = factions.GetRange(0, 2);
            var factionPlayer2 = factions.GetRange(2, 2);

            var players = new List<Player>
            {
                new()
                {
                    Name = "mesi",
                    IsActive = true,
                    Factions = factionPlayer1
                },
                new()
                {
                    Name = "keren",
                    IsActive = true,
                    Factions = factionPlayer2
                }
            };

            return players;
        }

        private void InitialPlayerUIs()
        {
            foreach (var player in _players)
            {
                var ui = _uiManager.CreatePlayerUI();
                ui.SetPlayerUI(player.Name
                    , player.Factions.Select(f => f.Color).ToList()
                    , OnTurnTimerExpired);

                player.UI = ui;
            }
        }

        private List<Faction> InitializeFactions()
        {
            var factions = FindObjectsOfType<Faction>().ToList();

            foreach (var faction in factions)
            {
                faction.Pawns = new List<IPawn>();
                faction.BaseNodes.ForEach(baseNode =>
                {
                    var newPawn = _pawnManager.CreatePawn(faction, baseNode);
                    newPawn.OnSelectPawn += OnSelectedPawn;
                });
            }

            return factions;
        }

        private void OnSelectedPawn(IPawn pawn)
        {
            if (!_isDiceRolled)
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
            _isDiceRolled = false;
            _diceManager.Reset();

            if (_turnLogicService.HasReward)
            {
                _gameFlowService.GrantReward(CurrentPlayer);
            }
            else
            {
                _gameFlowService.EndTurn(CurrentPlayer);
                SwitchTurn();
            }
        }

        private void OnDiceRolled(int? step)
        {
            if (step is null) return;

            _isDiceRolled = true;

            _diceManager.SetActivateDice(false);

            CurrentPlayer.UI.StopTimer();
            CurrentPlayer.UI.StartTurnTimer(10);

            if (step == 6)
                HandleFirstSixVisual();

            var canEnter = _playerActionValidator.CheckToEnterPawn(CurrentPlayer);
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

        private void HandleFirstSixVisual()
        {
            if (_firstSix) return;

            _firstSix = true;
            _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, _lastPlayer);
            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, _lastPlayer);
        }


        private async void SwitchTurn()
        {
            CurrentPlayer.UI.StopTimer();
            _lastPlayer = CurrentPlayer;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            _isDiceRolled = false;

            await _gameFlowService.DelayBetweenTurns(1000);

            _diceManager.Reset();
            _gameFlowService.StartTurn(CurrentPlayer);
            _diceManager.SetActivateDice(true);
            OnTurnSwitched?.Invoke();
        }

        private void OnTurnTimerExpired()
        {
            SwitchTurn();
        }
    }
}