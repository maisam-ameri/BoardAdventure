using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.State;
using UnityEngine;

namespace Core.Board
{
    public class FakeBoardDefinition : IBoardDefinition
    {
        public bool IsFinalGoal { get; set; }
        public List<INode> Nodes { get; set; }

        public FakeBoardDefinition( List<INode> nodes)
        {
            Nodes = new List<INode>();
            Nodes = nodes;
        }

        public FakeBoardDefinition()
        {
        }

        public bool IsFinalGoalNode(int? nodeId, FactionType factionType)
        {
            return IsFinalGoal;
        }

        public byte GetStartNode(FactionType factionType)
        {
            return Nodes.First(n =>
                n.NodeType == NodeType.Start
                && n.FactionType == factionType).NodeId;
        }
    }
}