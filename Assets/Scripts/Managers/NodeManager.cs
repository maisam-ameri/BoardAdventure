using System.Collections.Generic;
using System.Linq;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class NodeManager : MonoBehaviour
    {
        [SerializeField] private PathNode firstNode;
        [SerializeField] private LayerMask layerMask;
        private INode _blueStartNode;
        private INode _redStartNode;
        private INode _yellowStartNode;
        private INode _greenStartNode;
        private List<INode> _blueGoalNodes;
        private List<INode> _redGoalNodes;
        private List<INode> _yellowGoalNodes;
        private List<INode> _greenGoalNodes;
        private List<INode> _nodes;


        private void Start()
        {
            SetupNodes();
            SetupGoalNodes();
        }

        private void SetupNodes()
        {
            _nodes = new List<INode> {firstNode};
            firstNode.Index = 0;

            var allNodes = FindObjectsOfType<Node>()
                .Where(n => n is not BaseNode)
                .OrderBy(n => n.Collider2D.name)
                .Where(n => n is not Goal)
                .ToList();


            var nodeCounter = 0;

            while (true)
            {
                if (nodeCounter++ > allNodes.Count())
                    break;

                var currentNode = _nodes[^1];

                currentNode.Collider2D.enabled = false;

                var nextNode = FindNextNode(currentNode);

                currentNode.Collider2D.enabled = true;

                if (nextNode == null) break;

                currentNode.NextNode = nextNode;
                nextNode.PrevNode = currentNode;


                _nodes.Add(nextNode);
                nextNode.Index = _nodes.Count - 1;
            }

            _nodes.ForEach(n => n.Collider2D.enabled = false);
        }


        private INode FindNextNode(INode currentNode)
        {
            Vector2[] directions = {Vector2.right, Vector2.left, Vector2.up, Vector2.down};

            foreach (var dir in directions)
            {
                var hit = Physics2D.Raycast(currentNode.Position, dir, 1, layerMask);
                var node = hit.collider?.GetComponent<INode>();

                if (node is null) continue;
                if (node is Goal) continue;
                if (_nodes.Count == 1 && node is Gateway) continue;
                if (currentNode.PrevNode == node) continue;

                return node;
            }

            return null;
        }

        private void SetupGoalNodes()
        {
            var factions = FindObjectsOfType<Faction>();

            foreach (var faction in factions)
            {
                var goalsLen = faction.GoalNodes.Count  - 1;
            
                for (var i = 0; i <= goalsLen; i++)
                {
                    faction.GoalNodes[i].NextNode = i < goalsLen ? faction.GoalNodes[i + 1] : null;
                    faction.GoalNodes[i].PrevNode = i == 0 ? faction.GatewayNode : faction.GoalNodes[i - 1];
                }
            }
        }
    }
}