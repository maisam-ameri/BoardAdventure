using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Factions;
using Movement;
using Nodes;
using Nodes.Abstractions;
using Pawns;
using Players;
using UnityEngine;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        private DiceManager _diceManager;
        private PawnManager _pawnManager;
        private IMovement _mover;
        private bool _canEnterPawn;
        private bool _canMovePawn;
        private List<Player> _players;
        private int _currentPlayerIndex;

        public Player CurrentPlayer
        {
            get => _players[_currentPlayerIndex];
            set { }
        }
/*

 subscribe move event in mover
 define path
 check last node in path to Move or Switch Turn or Capture
 handle click on a factions
 handle click on a pawn
 enter a pawn to the game
 get free node from the base
 get pawn from the base
 on dice rolled => decide the player can move or enter a pawn (6) and just move(any number except 6)
 */

        private void Start()
        {
            InitializeManagers();
            var factions = InitializeFactions();
            _players = CreatePlayer(factions);
            CurrentPlayer = _players[0];


            _mover = new Mover(200, () =>
                {
                    var emptyNodes = factions.Where(
                        f => f.GoalNodes.Any(n => n.IsEmpty));

                    if (!emptyNodes.Any()) Debug.Log($"{CurrentPlayer.Name} won");
                }
            );
        }

        private void InitializeManagers()
        {
            _diceManager = FindObjectOfType<DiceManager>();
            _diceManager.OnDiceRolled += OnDiceRolled;
            _pawnManager = FindObjectOfType<PawnManager>();
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

            factions.ForEach(f => f.OnSelectFaction += OnSelectedFaction);


            foreach (var faction in factions)
            {
                faction.Pawns = new List<IPawn>();

                faction.BaseNodes.ForEach(baseNode => { CreatePawn(faction, baseNode); });
            }

            return factions;
        }

        private void CreatePawn(Faction faction, INode baseNode)
        {
            var newPawn = _pawnManager.CreatePawn(
                new PawnDataForCreate
                {
                    Color = faction.Color,
                    Position = baseNode.Position
                }
            );

            newPawn.OnSelectPawn += OnSelectedPawn;
            newPawn.Faction = faction;
            newPawn.Collider.enabled = false;
            newPawn.CurrentNode = baseNode;
            faction.Pawns.Add(newPawn);
            baseNode.IsEmpty = false;
        }

        private List<INode> DefinePath(int? step, IPawn pawn)
        {
            if (step == null) return null;

            var path = new List<INode>();
            var currentNode = pawn.CurrentNode;

            var remainedSteps = step.Value;

            while (remainedSteps > 0)
            {
                if (currentNode is Gateway gateway)
                {
                    if (pawn.Faction.GatewayNode == gateway)
                    {
                        currentNode = pawn.Faction.GoalNodes[0];
                        path.Add(currentNode);
                        remainedSteps--;
                    }
                    else
                    {
                        currentNode = gateway.NextNode?.NextNode;
                        if (currentNode == null) break;
                        path.Add(currentNode);
                        remainedSteps--;
                    }
                }
                else if (currentNode is Goal)
                {
                    var currentIndex = pawn.Faction.GoalNodes.IndexOf((Node) currentNode);
                    var remainedStepsInGoalArea = (pawn.Faction.GoalNodes.Count - 1) - currentIndex;
                    if (remainedSteps > remainedStepsInGoalArea)
                        break;

                    currentNode = currentNode.NextNode ?? currentNode;
                    path.Add(currentNode);
                    remainedSteps--;
                }
                else
                {
                    currentNode = currentNode.NextNode;

                    if (currentNode == null) break;

                    path.Add(currentNode);
                    remainedSteps--;
                }
            }

            var pathh = path.Distinct().ToList();

            var canMove = CheckNodeStateToMove(pathh.Count > 0 ? pathh[^1] : null);
            return canMove ? pathh : null;
        }

        private bool CheckNodeStateToMove(INode node)
        {
            if (node == null) return false;

            if (node.Pawn is null)
            {
                return node.IsEmpty;
            }

            if (CurrentPlayer.Factions.Any(f => f == node.Pawn.Faction))
            {
                SwitchTurn();
                return false;
            }

            Capture(node.Pawn);
            return false;
        }

        private void OnSelectedFaction(Faction faction)
        {
            if (CurrentPlayer.Factions.All(f => f != faction)) return;

            if (!_canEnterPawn)
            {
                Debug.LogWarning($"{CurrentPlayer.Name} isn't allowed to bring pawns into the game");
                return;
            }


            if (!faction.StartNode.IsEmpty)
            {
                Debug.LogWarning("the start node Dosn't empty ");
                return;
            }

            var pawn = GetPawnFromBase(faction);

            if (pawn == null)
            {
                Debug.LogWarning("Dosn't exist any pawns in the base");
            }
            else
            {
                _canEnterPawn = false;
                _canMovePawn = false;
                EnterPawnToGame((Pawn) pawn);
            }
        }

        private void OnSelectedPawn(IPawn pawn)
        {
            if (CurrentPlayer.Factions.All(f => f != pawn.Faction)) return;

            if (!_canMovePawn)
            {
                Debug.LogWarning($"{CurrentPlayer.Name} isn't allowed to move pawns");
                return;
            }

            var path = DefinePath(_diceManager.Step, pawn);
            if (path is null)
            {
                Debug.Log("you can't move");
            }
            else
            {
                _canEnterPawn = false;
                _canMovePawn = false;
                _mover.Move(pawn, path);
            }
        }

        private void EnterPawnToGame(Pawn pawn)
        {
            pawn.Position = pawn.Faction.StartNode.Position;
            pawn.CurrentNode.IsEmpty = true;
            pawn.CurrentNode = pawn.Faction.StartNode;
            pawn.Faction.StartNode.IsEmpty = false;
            pawn.Collider.enabled = true;
            pawn.State = "InGame";
        }

        private void OnDiceRolled(int? step)
        {
            if (step == 6)
            {
                _canEnterPawn = true;
                _canMovePawn = true;
            }
            else
            {
                _canMovePawn = true;
            }
        }

        private void SwitchTurn()
        {
            Debug.LogWarning("The Turn switched");

            _currentPlayerIndex = _currentPlayerIndex < _players.Count ? _currentPlayerIndex++ : 0;
        }

        private void Capture(IPawn pawn)
        {
            Debug.LogWarning($"Capture {pawn.Color}");
            return;

            var emptyBaseNode = GetEmptyNodeBase(pawn.Faction);

            if (emptyBaseNode is null)
            {
                Debug.LogWarning("There is no empty node in the base");
                return;
            }

            pawn.Position = emptyBaseNode.Position;
            pawn.CurrentNode = emptyBaseNode;
            emptyBaseNode.Pawn = pawn;
            emptyBaseNode.IsEmpty = false;
            pawn.Collider.enabled = false;
            pawn.State = "InBase";
        }

        private INode GetEmptyNodeBase(Faction faction)
            => faction.BaseNodes.FirstOrDefault(p => p.IsEmpty);

        private IPawn GetPawnFromBase(Faction faction)
            => faction.Pawns.FirstOrDefault(p => p.State == "InBase");
    }
}