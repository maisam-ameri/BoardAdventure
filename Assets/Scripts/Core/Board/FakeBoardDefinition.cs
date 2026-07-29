using BoardAdventures.Core.Board;
using BoardAdventures.Core.State;

namespace Core.Board
{
    public class FakeBoardDefinition: IBoardDefinition
    {
        public bool IsFinalGoal { get; set; }
        public bool IsFinalGoalNode(int? nodeId, FactionType factionType)
        {
            return IsFinalGoal;
        }
    }
}