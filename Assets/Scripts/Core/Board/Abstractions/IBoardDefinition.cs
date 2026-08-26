using System.Collections.Generic;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Board
{
    public interface IBoardDefinition
    {
        public List<INode> Nodes { get; set; }
        
        bool IsFinalGoalNode(int? nodeId, FactionType factionType);
        byte GetStartNode(FactionType factionType);
    }
}