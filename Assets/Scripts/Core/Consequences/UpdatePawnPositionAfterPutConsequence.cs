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
            matchState.BoardState.PawnsState
                .First(p => p.PawnId == putPawnResult.PawnId).NodeId = putPawnResult.NodeId;
        }
    }
}