// namespace Abstractions
// {
//     public interface IMovement
//     {
//         Task Move(IPawn pawn, List<INode> path);
//     }
// }
//
// namespace Dices
// {
//     public class Dice
//     {
//         public int Roll() => Random.Range(1, 7);
//     }
// }
//
// namespace Dices
// {
//     public class DiceUI : MonoBehaviour
//     {
//         [SerializeField] private TextMeshProUGUI stepTxt;
//         [SerializeField] private Button rollButton;
//         public void RollDice(int step)
//         {
//             stepTxt.text = step.ToString();
//         }
//
//         public void ActivateRoll(bool isActive)
//         {
//             rollButton.enabled = isActive;
//         }
//         
//     }
// }
//
// namespace Factions
// {
//     public class Faction : MonoBehaviour
//     {
//         public string Name;
//         public Color Color = Color.red;
//         public Node StartNode;
//         public Node GatewayNode;
//         public List<Node> GoalNodes;
//         public List<Node> BaseNodes;
//         public bool IsCompletedGoals { get; set; }
//         public List<IPawn> Pawns { get; set; }
//
//         public event Action<Faction> OnSelectFaction;
//         public event Action OnCompletedGoals;
//
//         private void OnMouseDown()
//         {
//             if(OnSelectFaction == null) Debug.Log("it is null");
//             OnSelectFaction?.Invoke(this);
//         }
//     }
// }
//
// namespace Managers
// {
//     public class DiceManager : MonoBehaviour
//     {
//         [SerializeField] private DiceUI diceUI;
//         private Dice _dice;
//
//         public event Action<int?> OnDiceRolled;
//         public int? Step { get; private set; }
//
//
//         private void Start()
//         {
//             _dice = new Dice();
//         }
//
//         public void RollDice()
//         {
//             Step = _dice.Roll();
//             diceUI.RollDice(Step.Value);
//             OnDiceRolled?.Invoke(Step);
//         }
//     }
// }
//
// namespace Managers
// {
//     public class NodeManager : MonoBehaviour
//     {
//         [SerializeField] private PathNode firstNode;
//         [SerializeField] private LayerMask layerMask;
//         [SerializeField] private INode blueStartNode;
//         [SerializeField] private INode redStartNode;
//         [SerializeField] private INode yellowStartNode;
//         [SerializeField] private INode greenStartNode;
//         [SerializeField] private List<INode> blueGoalNodes;
//         [SerializeField] private List<INode> redGoalNodes;
//         [SerializeField] private List<INode> yellowGoalNodes;
//         [SerializeField] private List<INode> greenGoalNodes;
//
//         private List<INode> _nodes;
//
//
//         private void Start()
//         {
//             SetupNodes();
//             SetupGoalNodes();
//         }
//
//
//         private void SetupNodes()
//         {
//             _nodes = new List<INode> {firstNode};
//             firstNode.Index = 0;
//
//             var allNodes = FindObjectsOfType<Node>()
//                 .Where(n => n is not BaseNode)
//                 .OrderBy(n => n.Collider2D.name)
//                 .Where(n => n is not Goal)
//                 .ToList();
//
//
//             var nodeCounter = 0;
//
//             while (true)
//             {
//                 if (nodeCounter++ > allNodes.Count())
//                 {
//                     Debug.LogError("SetupNodes stopped: possible infinite loop!");
//                     break;
//                 }
//
//                 var currentNode = _nodes[^1];
//
//                 currentNode.Collider2D.enabled = false;
//
//                 var nextNode = FindNextNode(currentNode);
//
//                 currentNode.Collider2D.enabled = true;
//
//                 if (nextNode == null) break;
//
//                 currentNode.NextNode = nextNode;
//                 nextNode.PrevNode = currentNode;
//
//
//                 _nodes.Add(nextNode);
//                 nextNode.Index = _nodes.Count - 1;
//             }
//
//             _nodes.ForEach(n => n.Collider2D.enabled = false);
//         }
//
//
//         private INode FindNextNode(INode currentNode)
//         {
//             Vector2[] directions = {Vector2.right, Vector2.left, Vector2.up, Vector2.down};
//
//             foreach (var dir in directions)
//             {
//                 var hit = Physics2D.Raycast(currentNode.Position, dir, 1, layerMask);
//                 var node = hit.collider?.GetComponent<INode>();
//
//                 if (node is null) continue;
//                 if (node is Goal) continue;
//                 if (_nodes.Count == 1 && node is Gateway) continue;
//                 if (currentNode.PrevNode == node) continue;
//
//                 return node;
//             }
//
//             return null;
//         }
//
//         private void SetupGoalNodes()
//         {
//             var factions = FindObjectsOfType<Faction>();
//
//             foreach (var faction in factions)
//             {
//                 var goalsLen = faction.GoalNodes.Count  - 1;
//             
//                 for (var i = 0; i <= goalsLen; i++)
//                 {
//                     faction.GoalNodes[i].NextNode = i < goalsLen ? faction.GoalNodes[i + 1] : null;
//                     faction.GoalNodes[i].PrevNode = i == 0 ? faction.GatewayNode : faction.GoalNodes[i - 1];
//                 }
//             }
//         }
//     }
// }
//
// namespace Managers
// {
//     public class PawnFactory
//     {
//         private readonly Pawn _pawn;
//
//         public PawnFactory(Pawn pawn)
//         {
//             _pawn = pawn;
//         }
//
//         public IPawn Create() => Object.Instantiate(_pawn);
//     }
// }
//
// namespace Managers
// {
//     public class PawnManager : MonoBehaviour
//     {
//         [SerializeField] private Pawn pawnPrefab;
//         private PawnFactory _pawnFactory;
//
//         private void Start()
//         {
//             _pawnFactory = new PawnFactory(pawnPrefab);
//         }
//
//         public IPawn CreatePawn(PawnDataForCreate pawnData)
//         {
//             var newPawn = _pawnFactory.Create();
//             newPawn.Position = pawnData.Position;
//             newPawn.Color = pawnData.Color;
//             newPawn.State = "InBase";
//
//             return newPawn;
//         }
//     }
// }
//
// namespace Managers
// {
//     public class TurnManager : MonoBehaviour
//     {
//         private DiceManager _diceManager;
//         private IMovement _mover;
//         private bool _canEnterPawn;
//         private bool _canMovePawn;
//         private List<Player> _players;
//         private int _currentPlayerIndex;
//
//         public Player CurrentPlayer
//         {
//             get => _players[_currentPlayerIndex];
//             set { }
//         }
//
//         private void Start()
//         {
//             _diceManager = FindObjectOfType<DiceManager>();
//             _diceManager.OnDiceRolled += OnDiceRolled;
//             var pawnManager = FindObjectOfType<PawnManager>();
//
//             var factions = FindObjectsOfType<Faction>().ToList();
//
//             factions.ForEach(f => f.OnSelectFaction += OnSelectedFaction);
//
//             _players = CreatePlayerForTest(factions);
//
//             CurrentPlayer = _players[0];
//
//             foreach (var player in _players)
//             {
//                 foreach (var faction in player.Factions)
//                 {
//                     faction.Pawns = new List<IPawn>();
//
//                     faction.BaseNodes.ForEach(baseNode =>
//                     {
//                         var newPawn = pawnManager.CreatePawn(
//                             new PawnDataForCreate
//                             {
//                                 Color = faction.Color,
//                                 Position = baseNode.Position
//                             }
//                         );
//
//                         newPawn.OnSelectPawn += OnSelectedPawn;
//                         newPawn.Faction = faction;
//                         newPawn.Collider.enabled = false;
//                         newPawn.CurrentNode = baseNode;
//                         faction.Pawns.Add(newPawn);
//                         baseNode.IsEmpty = false;
//                     });
//                 }
//             }
//
//             _mover = new Mover(200, () =>
//                 {
//                     var emptyNodes = factions.Where(
//                         f => f.GoalNodes.Any(n => n.IsEmpty));
//
//                     if (!emptyNodes.Any()) Debug.Log($"{CurrentPlayer.Name} won");
//                 }
//             );
//         }
//
//         private List<Player> CreatePlayerForTest(List<Faction> factions)
//         {
//             var factionPlayer1 = factions.GetRange(0, 2);
//             var factionPlayer2 = factions.GetRange(2, 2);
//
//             var players = new List<Player>
//             {
//                 new()
//                 {
//                     Name = "mesi",
//                     Order = 0,
//                     IsActive = true,
//                     Factions = factionPlayer1
//                 },
//                 new()
//                 {
//                     Name = "keren",
//                     Order = 0,
//                     IsActive = true,
//                     Factions = factionPlayer2
//                 }
//             };
//
//             return players;
//         }
//
//         private List<INode> DefinePath(int? step, IPawn pawn)
//         {
//             if (step == null) return null;
//
//             var path = new List<INode>();
//             var currentNode = pawn.CurrentNode;
//
//             var remainedSteps = step.Value;
//
//             while (remainedSteps > 0)
//             {
//                 if (currentNode is Gateway gateway)
//                 {
//                     if (pawn.Faction.GatewayNode == gateway)
//                     {
//                         currentNode = pawn.Faction.GoalNodes[0];
//                         path.Add(currentNode);
//                         remainedSteps--;
//                     }
//                     else
//                     {
//                         currentNode = gateway.NextNode?.NextNode;
//                         if (currentNode == null) break;
//                         path.Add(currentNode);
//                         remainedSteps--;
//                     }
//                 }
//                 else if (currentNode is Goal)
//                 {
//                     var currentIndex = pawn.Faction.GoalNodes.IndexOf((Node) currentNode);
//                     var remainedStepsInGoalArea = (pawn.Faction.GoalNodes.Count - 1) - currentIndex;
//                     if (remainedSteps > remainedStepsInGoalArea)
//                         break;
//
//                     currentNode = currentNode.NextNode ?? currentNode;
//                     path.Add(currentNode);
//                     remainedSteps--;
//                 }
//                 else
//                 {
//                     currentNode = currentNode.NextNode;
//
//                     if (currentNode == null) break;
//
//                     path.Add(currentNode);
//                     remainedSteps--;
//                 }
//             }
//
//             var pathh = path.Distinct().ToList();
//
//             var canMove = CheckNodeStateToMove(pathh.Count > 0 ? pathh[^1] : null);
//             return canMove ? pathh : null;
//         }
//
//         private bool CheckNodeStateToMove(INode node)
//         {
//             if (node == null) return false;
//
//             if (node.Pawn is null)
//             {
//                 return node.IsEmpty;
//             }
//
//             if (CurrentPlayer.Factions.Any(f => f == node.Pawn.Faction))
//             {
//                 SwitchTurn();
//                 return false;
//             }
//
//             Capture(node.Pawn);
//             return false;
//         }
//
//         private void OnSelectedFaction(Faction faction)
//         {
//             if (CurrentPlayer.Factions.All(f => f != faction)) return;
//
//             if (!_canEnterPawn)
//             {
//                 Debug.LogWarning($"{CurrentPlayer.Name} isn't allowed to bring pawns into the game");
//                 return;
//             }
//
//
//             if (!faction.StartNode.IsEmpty)
//             {
//                 Debug.LogWarning("the start node Dosn't empty ");
//                 return;
//             }
//
//             var pawn = GetPawnFromBase(faction);
//
//             if (pawn == null)
//             {
//                 Debug.LogWarning("Dosn't exist any pawns in the base");
//             }
//             else
//             {
//                 _canEnterPawn = false;
//                 _canMovePawn = false;
//                 EnterPawnToGame((Pawn) pawn);
//             }
//         }
//
//         private void OnSelectedPawn(IPawn pawn)
//         {
//             if (CurrentPlayer.Factions.All(f => f != pawn.Faction)) return;
//
//             if (!_canMovePawn)
//             {
//                 Debug.LogWarning($"{CurrentPlayer.Name} isn't allowed to move pawns");
//                 return;
//             }
//
//             var path = DefinePath(_diceManager.Step, pawn);
//             if (path is null)
//             {
//                 Debug.Log("you can't move");
//             }
//             else
//             {
//                 _canEnterPawn = false;
//                 _canMovePawn = false;
//                 _mover.Move(pawn, path);
//             }
//         }
//
//         private void EnterPawnToGame(Pawn pawn)
//         {
//             pawn.Position = pawn.Faction.StartNode.Position;
//             pawn.CurrentNode.IsEmpty = true;
//             pawn.CurrentNode = pawn.Faction.StartNode;
//             pawn.Faction.StartNode.IsEmpty = false;
//             pawn.Collider.enabled = true;
//             pawn.State = "InGame";
//         }
//
//         private void OnDiceRolled(int? step)
//         {
//             
//             if (step == 6)
//             {
//                 _canEnterPawn = true;
//                 _canMovePawn = true;
//             }
//             else
//             {
//                 _canMovePawn = true;
//             }
//         }
//
//         private void SwitchTurn()
//         {
//             Debug.LogWarning("The Turn switched");
//
//             _currentPlayerIndex = _currentPlayerIndex < _players.Count ? _currentPlayerIndex++ : 0;
//         }
//
//         private void Capture(IPawn pawn)
//         {
//             Debug.LogWarning($"Capture {pawn.Color}");
//             return;
//
//             var emptyBaseNode = GetEmptyNodeBase(pawn.Faction);
//
//             if (emptyBaseNode is null)
//             {
//                 Debug.LogWarning("There is no empty node in the base");
//                 return;
//             }
//
//             pawn.Position = emptyBaseNode.Position;
//             pawn.CurrentNode = emptyBaseNode;
//             emptyBaseNode.Pawn = pawn;
//             emptyBaseNode.IsEmpty = false;
//             pawn.Collider.enabled = false;
//             pawn.State = "InBase";
//         }
//
//         private INode GetEmptyNodeBase(Faction faction)
//             => faction.BaseNodes.FirstOrDefault(p => p.IsEmpty);
//
//         private IPawn GetPawnFromBase(Faction faction)
//             => faction.Pawns.FirstOrDefault(p => p.State == "InBase");
//     }
// }
//
// namespace Movement
// {
//     public class Mover : IMovement
//     {
//
//         private readonly int _delay;
//         private event Action OnMoveCompleted;
//         
//         
//         public Mover(int delay = 1000,Action onMoveCompleted = null)
//         {
//             OnMoveCompleted = onMoveCompleted;
//             _delay = delay;
//         }
//         public async Task Move(IPawn pawn, List<INode> path)
//         {
//             var maxStep = path.Count ;
//             var currentStep = 0;
//
//             path[currentStep].PrevNode.IsEmpty = true;
//             path[currentStep].PrevNode.Pawn = null;
//             
//             while (currentStep < maxStep)
//             {
//                 pawn.Position = path[currentStep].Position;
//                 currentStep++;
//
//                 await Task.Delay(_delay);
//             }
//
//             pawn.CurrentNode = path[^1];
//             pawn.CurrentNode.Pawn = pawn;
//             path[^1].IsEmpty = false;
//          
//             OnMoveCompleted?.Invoke();
//
//
//         }
//     }
// }
//
// namespace Nodes.Abstractions
// {
//     public interface INode
//     {
//         int Index { get; set; }
//         bool IsEmpty { get; set; }
//         INode PrevNode { get; set; }
//         INode NextNode { get; set; }
//         Vector2 Position { get; set; }
//         Collider2D Collider2D { get; }
//         public IPawn Pawn { get; set; }
//     }
// }
//
// namespace Nodes
// {
//     public class BaseNode: Node
//     {
//         
//     }
// }
//
// namespace Nodes
// {
//     public class Gateway: Node
//     {
//
//     }
// }
//
// namespace Nodes
// {
//     public class Goal: Node
//     {
//
//     }
// }
//
// namespace Nodes
// {
//     public abstract class Node : MonoBehaviour, INode
//     {
//         public int Index { get; set; }
//         public bool IsEmpty { get; set; } = true;
//         public INode PrevNode { get; set; }
//         public INode NextNode { get; set; }
//
//         public Collider2D Collider2D => GetComponent<Collider2D>();
//         
//         public IPawn Pawn { get; set; }
//     
//         public Vector2 Position
//         {
//             get => transform.position;
//             set => transform.position = value;
//         }
//
//
//     }
// }
//
// namespace Nodes
// {
//     public class PathNode: Node
//     {
//
//     }
// }
//
// namespace Pawns
// {
//     public interface IPawn
//     {
//         string State { get; set; }
//         Color Color { get; set; }
//         Vector2 Position { get; set; }
//         Faction Faction { get; set; }
//         INode CurrentNode { get; set; }
//         public Collider2D Collider { get; }
//
//         event Action<IPawn> OnSelectPawn;
//     }
// }
//
// namespace Pawns
// {
//     public class Pawn : MonoBehaviour, IPawn
//     {
//         public string State { get; set; }
//
//         public Color Color
//         {
//             get => GetComponent<SpriteRenderer>().color;
//             set => GetComponent<SpriteRenderer>().color = value;
//         }
//
//         public Vector2 Position
//         {
//             get => transform.position;
//             set => transform.position = value;
//         }
//
//         public Faction Faction { get; set; }
//         public INode CurrentNode { get; set; }
//         public Collider2D Collider => GetComponent<Collider2D>();
//
//         public event Action<IPawn> OnSelectPawn;
//
//         private void OnMouseDown()
//         {
//             OnSelectPawn?.Invoke(this);
//         }
//     }
// }
//
// namespace Pawns
// {
//     public class PawnDataForCreate
//     {
//         public Color Color { get; set; }
//         public Vector2 Position { get; set; }
//     }
// }
//
// namespace Players
// {
//     public  class Player
//     {
//         public string Name { get; set; }
//         public bool IsActive { get; set; }
//         public int Order { get; set; }
//
//         public List<Faction> Factions { get; set; }
//     }
// }