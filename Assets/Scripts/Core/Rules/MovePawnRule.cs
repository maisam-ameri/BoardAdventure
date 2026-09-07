using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using JetBrains.Annotations;

namespace BoardAdventures.Core.Rules
{
    public class MovePawnRule
    {
        private readonly IPathCalculator _pathCalculator;

        public MovePawnRule(IPathCalculator pathCalculator)
        {
            _pathCalculator = pathCalculator;
        }

        public MovePawnResult Execute(MatchState state, MovePawnCommand command, CommandContext context)
        {
            var pawn = state.BoardState.PawnsState.FirstOrDefault(p => p.PawnId == command.PawnId);
            var pawnId = command.PawnId;
            var path = _pathCalculator.Calculate(pawn, state.DiceState.Step);

            var result = ValidatePawnExist(pawn, pawnId);
            if (result != null)
                return result;

            result = ValidatePlayersTurn(state, context, pawnId);
            if (result != null)
                return result;

            result = ValidatePawnOwnership(pawn, pawnId, context);
            if (result != null)
                return result;

            result = ValidatePath(pawnId, path);
            if (result != null)
                return result;

            result = ValidateDestination(state, pawnId, path,context);
            if (result != null)
                return result;


            return new MovePawnResult(pawnId, MoveFailReason.None, path);
        }

        [CanBeNull]
        private MovePawnResult ValidatePawnExist(PawnState pawn, byte pawnId)
        {
            return pawn is null ? Fail(pawnId, MoveFailReason.PawnNotFound) : null;
        }

        [CanBeNull]
        private MovePawnResult ValidatePlayersTurn(MatchState state, CommandContext context, byte pawnId)
        {
            return state.TurnState.CurrentPlayerId != context.PlayerId
                ? Fail(pawnId, MoveFailReason.NotPlayersTurn)
                : null;
        }

        [CanBeNull]
        private MovePawnResult ValidatePawnOwnership(PawnState pawn, byte pawnId, CommandContext context
        )
        {
            return pawn.OwnerPlayerId != context.PlayerId
                ? Fail(pawnId, MoveFailReason.PawnDoesNotBelongToPlayer)
                : null;
        }

        [CanBeNull]
        private MovePawnResult ValidatePath(byte pawnId, IReadOnlyList<byte> path)
        {
            return path.Count == 0 ? Fail(pawnId, MoveFailReason.InvalidPath) : null;
        }

        [CanBeNull]
        private MovePawnResult ValidateDestination(MatchState state, byte pawnId, IReadOnlyList<byte> path,
            CommandContext context)
        {
            var isDestinationOccupiedByTeammate = state.BoardState.PawnsState
                .Any(p =>
                    p.PawnId != pawnId
                    && p.OwnerPlayerId == context.PlayerId
                    && p.NodeId == path.Last());

            return isDestinationOccupiedByTeammate ? Fail(pawnId, MoveFailReason.DestinationOccupied) : null;
        }

        private MovePawnResult Fail(byte pawnId, MoveFailReason reason) => new(pawnId, reason);
   
    }
}