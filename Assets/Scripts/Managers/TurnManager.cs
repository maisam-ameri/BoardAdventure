using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Factions;
using GameLogic;
using Movement;
using Nodes.Abstractions;
using Path;
using Pawns;
using Players;
using UI;
using UnityEngine;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        private DiceManager _diceManager;
        private PawnManager _pawnManager;
        private UIManager _uiManager;
        private UIMessageManager _uiMessageManager;
        private TurnVisualizer _turnVisualizer;
        private PathCalculator _pathCalculator;
        private IMovement _mover;
        private List<Player> _players;
        private int _currentPlayerIndex;
        private bool _firstSix;
        private bool _isDiceRolled;
        private readonly TurnLogicService _turnLogicService = new ();

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
            _mover = new Mover(200);
        }

        private void InitializeManagers()
        {
            _diceManager = FindObjectOfType<DiceManager>();
            _diceManager.OnDiceRolled += OnDiceRolled;
            _pawnManager = FindObjectOfType<PawnManager>();
            _pathCalculator = new PathCalculator();
            _uiManager = FindObjectOfType<UIManager>();
            _turnVisualizer = FindObjectOfType<TurnVisualizer>();
            _uiMessageManager = FindObjectOfType<UIMessageManager>();
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

            if (pawn.State == "InBase" && _diceManager.Step is 6)
            {
                if (CurrentPlayer.Factions.All(f => f != faction)) return;

                if (!faction.StartNode.IsEmpty)
                {
                    _uiMessageManager.ShowStartNodeMessage();
                    return;
                }

                EnterPawnToGame((Pawn) pawn);
            }
            else if (pawn.State == "InGame")
            {
                _ = HandleSelectedPawnAsync(pawn);
            }
        }

        private async Task HandleSelectedPawnAsync(IPawn pawn)
        {
            var path = CheckPathIsValid(pawn, _diceManager.Step);
            if (path is null) return;

            await _mover.Move(pawn, path, OnActionCompleted);
        }

        private List<INode> CheckPathIsValid(IPawn pawn, int? step)
        {
            if (CurrentPlayer.Factions.All(f => f != pawn.Faction)) return null;

            var path = _pathCalculator.DefinePath(step, pawn);

            return path.Count != 0 && PathValidator.CanMoveToNode(path[^1], CurrentPlayer) ? path : null;
        }

        private void EnterPawnToGame(Pawn pawn)
        {
            pawn.Position = pawn.Faction.StartNode.Position;
            pawn.CurrentNode.IsEmpty = true;
            pawn.CurrentNode.Pawn = null;
            pawn.CurrentNode = pawn.Faction.StartNode;
            pawn.Faction.StartNode.IsEmpty = false;
            pawn.State = "InGame";
            OnActionCompleted();
        }

        private void OnActionCompleted()
        {
            _isDiceRolled = false;

            if (_turnLogicService.HasReward)
            {
                // show delay to active dice
                CurrentPlayer.UI.StartTurnTimer(10);
            }
            else
            {
                SwitchTurn();
                return;
            }

            _diceManager.Reset();
            _diceManager.SetActivateDice(true);
        }

        private bool _hasReward;

        private void OnDiceRolled(int? step)
        {
            if (step is null) return;
            
            _isDiceRolled = true;
            
            _diceManager.SetActivateDice(false);

            CurrentPlayer.UI.StopTimer();
            CurrentPlayer.UI.StartTurnTimer(10);

            if(step == 6 )
                HandleFirstSixVisual();
            
            var canEnter = CheckToEnterPawn();
            var canMove = CheckToMovePawn(step);

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
                    _diceManager.SetActivateDice(true);
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


        private bool CheckToMovePawn(int? step)
        {
            foreach (var faction in CurrentPlayer.Factions)
            {
                var pawns = GetPawnsFromGame(faction);
                foreach (var pawn in pawns)
                {
                    if (pawn is not null)
                    {
                        var path = CheckPathIsValid(pawn, step);

                        if (path is null) continue;

                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        private bool CheckToEnterPawn()
        {
            foreach (var faction in CurrentPlayer.Factions)
            {
                var isExistPawnInBase = faction.Pawns.Any(p => p.State == "InBase");
                var isStartNodeEmpty = faction.StartNode.IsEmpty;

                if (isExistPawnInBase && isStartNodeEmpty)
                    return true;
            }

            return false;
        }

        private void SwitchTurn()
        {
            CurrentPlayer.UI.StopTimer();
            _lastPlayer = CurrentPlayer;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

            _isDiceRolled = false;
            _diceManager.Reset();
            _diceManager.SetActivateDice(true);


            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, _lastPlayer);
            if (_firstSix)
                _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, _lastPlayer);


            CurrentPlayer.UI.StartTurnTimer(5);
            OnTurnSwitched?.Invoke();
        }

        private void OnTurnTimerExpired()
        {
            SwitchTurn();
        }

        private IEnumerable<IPawn> GetPawnsFromGame(Faction faction)
            => faction.Pawns.Where(p => p.State == "InGame");
    }
}