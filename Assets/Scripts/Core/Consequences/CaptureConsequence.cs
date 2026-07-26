using System.Linq;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Consequences
{
    public class CaptureConsequence
    {
        public bool Execute(MatchState state, MovePawnResult movePawnResult)
        {
            var currentPlayer = state.TurnState.CurrentPlayerId;
            var destinationNodeId = movePawnResult.Path[^1];
            var enemyPawn = state.BoardState.PawnsState.FirstOrDefault(p =>
                p.OwnerPlayerId != currentPlayer
                && p.NodeId == destinationNodeId);


            if (enemyPawn == null) return false;
            
            enemyPawn.PawnLocationState = PawnLocationState.InBase;
            enemyPawn.NodeId = null;
            return true;

        }
    }
}