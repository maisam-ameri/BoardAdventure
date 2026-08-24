using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.State;

namespace Core.Board
{
    public class FakeBoardDefinition : IBoardDefinition
    {
        public bool IsFinalGoal { get; set; }
        public List<INode> Nodes { get; set; }
        private readonly MatchState _matchState;

        public FakeBoardDefinition(MatchState matchState, List<INode> nodes)
        {
            _matchState = matchState;
            Nodes = nodes;
        }

        public FakeBoardDefinition()
        {
        }

        public bool IsFinalGoalNode(int? nodeId, FactionType factionType)
        {
            return IsFinalGoal;
        }

        public byte? GetFreeStartNode(FactionType factionType)
        {
            return Nodes.First(n =>
                n.NodeType == NodeType.Start
                && n.FactionType == factionType
                && !IsNodeOccupied(n.NodeId)).NodeId;
        }

        private bool IsNodeOccupied(byte nodeId)
        {
            return _matchState.BoardState.PawnsState.Any(p => p.NodeId == nodeId);
        }
    }
}