using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Board
{
    public interface IBoardDefinition
    {
        bool IsFinalGoalNode(int? nodeId, FactionType factionType);
    }
}