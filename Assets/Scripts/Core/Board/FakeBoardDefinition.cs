using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Board
{
    public class FakeBoardDefinition : IBoardDefinition
    {
        public bool IsFinalGoal { get; set; }
        public List<INode> Nodes { get; set; }

        public FakeBoardDefinition(List<INode> nodes)
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

        public byte? GetStartNode(FactionType factionType) => Nodes
            .FirstOrDefault(n => n.NodeType == NodeType.Start && n.FactionType == factionType)
            ?.NodeId;

        public List<INode> GetGoalNodes(FactionType faction) => Nodes
            .Where(n => n.FactionType == faction && n.NodeType == NodeType.Goal)
            .ToList();

        public INode GetNodeById(byte? id) => Nodes.FirstOrDefault(n => n.NodeId == id);
    }
}