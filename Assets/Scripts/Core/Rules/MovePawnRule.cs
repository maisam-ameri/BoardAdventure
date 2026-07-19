using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Rules
{
    public class MovePawnRule
    {
        public MovePawnResult Execute(MatchState state, MovePawnCommand command, CommandContext context)
        {
            var pawn = state.BoardState.PawnsState.FirstOrDefault(p => p.PawnId == command.PawnId);

            if (pawn is null)
            {
                return Fail(MoveFailReason.PawnNotFound);
            }

            if (state.TurnState.CurrentPlayerId != context.PlayerId)
            {
                return Fail(MoveFailReason.NotPlayersTurn);
            }

            if (pawn.OwnerPlayerId != context.PlayerId)
            {
                return Fail(MoveFailReason.PawnDoesNotBelongToPlayer);
            }

            // TODO: calculate path
            var path = CalculatePath(state.DiceState.Step);
            if (path.Count == 0)
            {
                return Fail(MoveFailReason.InvalidPath);
            }

            return new MovePawnResult(MoveFailReason.None, path);
        }

        private MovePawnResult Fail(MoveFailReason reason) => new(reason);

        IReadOnlyList<int> CalculatePath(byte? step)
        {
            return new[] {1, 2, 3, 4, 5};
        }
    }
}