using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Consequences
{
    public class FinalGoalReachedConsequence: IMoveConsequence
    {
        public int Order { get; } = ConsequenceOrder.FinalGoal;
        private readonly IBoardDefinition _boardDefinition;


        public FinalGoalReachedConsequence(IBoardDefinition boardDefinition)
        {
            _boardDefinition = boardDefinition;
        }


        public void Execute(MatchState state, MovePawnResult movePawnResult)
        {
            var pawn = state.BoardState.PawnsState.First(p => p.PawnId == movePawnResult.PawnId);
            var isFinalGoalNode = _boardDefinition.IsFinalGoalNode(movePawnResult.Path[^1], pawn.FactionType);

            if (!isFinalGoalNode) return;

            pawn.PawnLocationState = PawnLocationState.InFinalGoal;
        }
    }
}