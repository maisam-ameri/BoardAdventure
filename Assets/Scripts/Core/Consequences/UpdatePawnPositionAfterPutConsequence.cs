using System.Linq;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace Core.Consequences
{
    public class UpdatePawnPositionAfterPutConsequence
    {
        public int Order { get; } = ConsequenceOrder.UpdatePosition;
        public void Execute(MatchState matchState, PutPawnResult putPawnResult)
        {
            var pawn =  matchState.BoardState.PawnsState
                .First(p => p.PawnId == putPawnResult.PawnId);
            
            pawn.NodeId = putPawnResult.NodeId;
            pawn.PawnLocationState = PawnLocationState.OnBoard;
        }
    }
}