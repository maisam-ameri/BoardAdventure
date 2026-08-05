using System.Linq;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Consequences
{
    public class UpdatePawnPositionConsequence: IMoveConsequence
    {
        public int Order { get; } = ConsequenceOrder.UpdatePosition;
        public void Execute(MatchState matchState, MovePawnResult movePawnResult)
        {
            var lastNode = movePawnResult.Path.Last(); 
            
            matchState.BoardState.PawnsState
                .First(p => p.PawnId == movePawnResult.PawnId).NodeId = lastNode;
        }
    }
}