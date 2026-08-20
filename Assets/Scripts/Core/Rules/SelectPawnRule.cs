using System.Linq;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Rules
{
    public class SelectPawnRule
    {
        public SelectPawnResult Execute(MatchState state, SelectPawnCommand command, CommandContext context)
        {
            var selectedPawn = state.BoardState.PawnsState
                .First(p => p.OwnerPlayerId == context.PlayerId && p.PawnId == command.PawnId);
            var canMove = false;
            var canPut = false;
            
            if (state.DiceState.Step == 6 && selectedPawn.PawnLocationState == PawnLocationState.InBase)
            {
                canPut = true;
            }
            else if(selectedPawn.PawnLocationState == PawnLocationState.OnBoard)
            {
                canMove = true;
            }

            return new SelectPawnResult {CanMove = canMove, CanPut = canPut};
        }

    }
}