using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using UnityEngine;

namespace BoardAdventures.Core.Rules
{
    public class MovePawnRule
    {
        public MovePawnResult Execute(MatchState state, MovePawnCommand command, CommandContext context)
        {
            var pawn = state.BoardState.PawnsState.FirstOrDefault(p => p.PawnId == command.PawnId);
            var pawnId = command.PawnId;

            if (pawn is null)
            {
                return Fail(pawnId, MoveFailReason.PawnNotFound);
            }

            if (state.TurnState.CurrentPlayerId != context.PlayerId)
            {
                return Fail(pawnId, MoveFailReason.NotPlayersTurn);
            }

            if (pawn.OwnerPlayerId != context.PlayerId)
            {
                return Fail(pawnId, MoveFailReason.PawnDoesNotBelongToPlayer);
            }

            // TODO: calculate path
            var path = CalculatePath(state.DiceState.Step);

            if (path.Count == 0)
            {
                return Fail(pawnId, MoveFailReason.InvalidPath);
            }

            return new MovePawnResult(pawnId, MoveFailReason.None, path);
        }

        private MovePawnResult Fail(byte pawnId, MoveFailReason reason) => new(pawnId, reason);

        IReadOnlyList<int> CalculatePath(byte? step)
        {
            return new[] {1, 2, 3, 4, 5};
        }
    }
}