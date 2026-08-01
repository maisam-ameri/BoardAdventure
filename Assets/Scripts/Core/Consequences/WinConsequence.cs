using System.Linq;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Consequences
{
    public class WinConsequence: IMoveConsequence
    {
        public int Order { get; } = ConsequenceOrder.Win;
        public void Execute(MatchState matchState, MovePawnResult movePawnResult)
        {
            var playerId = matchState.BoardState.PawnsState
                .Find(p =>p.PawnId == movePawnResult.PawnId)
                .OwnerPlayerId;
            
            var arePawnsReachedGoalNode = matchState.BoardState.PawnsState
                .Where(p => p.OwnerPlayerId == playerId)
                .Select(p => p.PawnLocationState)
                .All(p => p == PawnLocationState.InFinalGoal);

            if (arePawnsReachedGoalNode)
                matchState.WinnerId = playerId;
        }
    }
}