using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Factions;
using Movement;
using Nodes.Abstractions;
using Path;
using Pawns;
using Players;
using UnityEngine;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        private DiceManager _diceManager;
        private PawnManager _pawnManager;
        private PathCalculator _pathCalculator;
        private IMovement _mover;
        private List<Player> _players;
        private int _currentPlayerIndex;
        private bool _firstSix;
        private bool _isDiceRolled;

        public Action OnTurnSwitched { get; set; }
        public Player CurrentPlayer
        {
            get => _players[_currentPlayerIndex];
            set { }
        }
        
        

        private void Start()
        {
            InitializeManagers();
            var factions = InitializeFactions();
            _players = CreatePlayer(factions);
            CurrentPlayer = _players[0];
            DeactivatePlayersVisual();
            _mover = new Mover(200);
        }

        private void InitializeManagers()
        {
            _diceManager = FindObjectOfType<DiceManager>();
            _diceManager.OnDiceRolled += OnDiceRolled;
            _pawnManager = FindObjectOfType<PawnManager>();
            _pathCalculator = new PathCalculator();
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

        private List<Faction> InitializeFactions()
        {
            var factions = FindObjectsOfType<Faction>().ToList();

            foreach (var faction in factions)
            {
                faction.OnSelectFaction += OnSelectedFaction;
                faction.Pawns = new List<IPawn>();
                faction.BaseNodes.ForEach(baseNode =>
                {
                    var newPawn = _pawnManager.CreatePawn(faction, baseNode);
                    newPawn.OnSelectPawn += OnSelectedPawn;
                });
            }

            return factions;
        }

        private void OnSelectedFaction(Faction faction)
        {
            if (!_isDiceRolled)
            {
                Debug.LogWarning("please roll");
                return;
            }
            
            if (CurrentPlayer.Factions.All(f => f != faction)) return;

            if (!faction.StartNode.IsEmpty)
            {
                Debug.LogWarning("Start node isn't empty");
                return;
            }

            var pawn = GetPawnFromBase(faction);
            EnterPawnToGame((Pawn) pawn);
        }

        private void OnSelectedPawn(IPawn pawn)
        {
            if (!_isDiceRolled)
            {
                Debug.LogWarning("please roll");
                return;
            }
            _ = HandleSelectedPawnAsync(pawn);
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
            pawn.CurrentNode = pawn.Faction.StartNode;
            pawn.Faction.StartNode.IsEmpty = false;
            pawn.Collider.enabled = true;
            pawn.State = "InGame";
            OnActionCompleted();
        }

        private void OnActionCompleted()
        {
            SwitchTurn();
        }

        private void OnDiceRolled(int? step)
        {
            _isDiceRolled = true;
            switch (step)
            {
                case null:
                    return;
                case 6:
                    if (!_firstSix)
                    {
                        _firstSix = true;
                        UpdatePlayersVisual();
                    }
                    var canEnterPawn = CheckToEnterPawn();
                    var canMovePawn = CheckToMovePawn(step);

                    if (canEnterPawn)
                        Debug.LogWarning($"{CurrentPlayer.Name} can enter a pawn");

                    if (canMovePawn)
                        Debug.LogWarning($"{CurrentPlayer.Name} can move a pawn");

                    if (!canEnterPawn && !canMovePawn)
                    {
                        SwitchTurn();
                        //UpdatePlayersVisual();
                    }

                    break;

                default:
                    if (CheckToMovePawn(step))
                        Debug.LogWarning($"{CurrentPlayer.Name} can move a pawn");
                    else
                    {
                        SwitchTurn();
                        //UpdatePlayersVisual();
                    }


                    break;
            }

            //UpdatePlayersVisual();
        }

        private void DeactivatePlayersVisual()
        {
            foreach (var player in _players)
            {
                var factions = player.Factions;

                //factions.ForEach(f => f.SetActivate(false));
                factions.ForEach(f => f.Pawns.ForEach(p => p.IsActive = false));
            }
        }

        private void UpdatePlayersVisual()
        {
            if (!_firstSix) return;

            foreach (var player in _players)
            {
                var factions = player.Factions;

                if (player == CurrentPlayer)
                {
                    //factions.ForEach(f => f.SetActivate(true));
                    factions.ForEach(f => f.Pawns.ForEach(p => p.IsActive = true));
                }
                else
                {
                    //factions.ForEach(f => f.SetActivate(false));
                    factions.ForEach(f => f.Pawns.ForEach(p => p.IsActive = false));
                }
            }
        }

        private bool CheckToMovePawn(int? step)
        {
            foreach (var faction in CurrentPlayer.Factions)
            {
                var pawn = GetPawnFromGame(faction);
                if (pawn is not null)
                {
                    return CheckPathIsValid(pawn, step) is not null;
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
            // a loop in the players
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            _isDiceRolled = false;
            UpdatePlayersVisual();
            OnTurnSwitched?.Invoke();
            Debug.LogWarning($"The Turn is {CurrentPlayer.Name}");
        }

        private IPawn GetPawnFromBase(Faction faction)
            => faction.Pawns.FirstOrDefault(p => p.State == "InBase");

        private IPawn GetPawnFromGame(Faction faction)
            => faction.Pawns.FirstOrDefault(p => p.State == "InGame");
    }
}