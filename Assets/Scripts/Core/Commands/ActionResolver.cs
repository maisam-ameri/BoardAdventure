using System;
using System.Linq;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Commands
{
    public class ActionResolver
    {
        public ICommand Resolve(MatchState state, byte pawnId)
        {
            var pawnLocation = state.BoardState.PawnsState
                .First(p => p.PawnId == pawnId)
                .PawnLocationState;

            switch (pawnLocation)
            {
                case PawnLocationState.OnBoard:
                    // create a move command
                    return new MovePawnCommand(pawnId);
                    break;
                // case PawnLocationState.InBase:
                //     // create a put command
                //     break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}