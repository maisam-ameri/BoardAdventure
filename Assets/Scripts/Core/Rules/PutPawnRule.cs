using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using JetBrains.Annotations;

namespace BoardAdventures.Core.Rules
{
    public class PutPawnRule
    {
        private readonly IBoardDefinition _boardDefinition;

        public PutPawnRule(IBoardDefinition boardDefinition)
        {
            _boardDefinition = boardDefinition;
        }

        public PutPawnResult Execute(MatchState state, PutPawnCommand command, CommandContext context)
        {
            var pawn = state.BoardState.PawnsState.FirstOrDefault(p => p.PawnId == command.PawnId);
            var pawnId = command.PawnId;
            byte? nodeId;

            var result = ValidatePawnExist(pawn, pawnId);
            if (result != null)
                return result;

            result = ValidatePlayersTurn(state, context, pawnId);
            if (result != null)
                return result;

            result = ValidatePawnOwnership(pawn, pawnId, context);
            if (result != null)
                return result;

            result = ValidateStartNode(state, pawnId, out nodeId);
            if (result != null)
                return result;


            return new PutPawnResult(pawnId, PutFailReason.None, nodeId);
        }

        [CanBeNull]
        private PutPawnResult ValidatePawnExist(PawnState pawn, byte pawnId)
        {
            return pawn is null ? Fail(pawnId, PutFailReason.PawnNotFound) : null;
        }

        [CanBeNull]
        private PutPawnResult ValidatePlayersTurn(MatchState state, CommandContext context, byte pawnId)
        {
            return state.TurnState.CurrentPlayerId != context.PlayerId
                ? Fail(pawnId, PutFailReason.NotPlayersTurn)
                : null;
        }

        [CanBeNull]
        private PutPawnResult ValidatePawnOwnership(PawnState pawn, byte pawnId, CommandContext context
        )
        {
            return pawn.OwnerPlayerId != context.PlayerId
                ? Fail(pawnId, PutFailReason.PawnDoesNotBelongToPlayer)
                : null;
        }

        private PutPawnResult ValidateStartNode(MatchState state, byte pawnId, out byte? nodeId)
        {
            var faction = state.BoardState.PawnsState.First(p => p.PawnId == pawnId).FactionType;
            var startNodeId = _boardDefinition.GetStartNode(faction);
            nodeId = startNodeId;

            return state.BoardState.PawnsState.Any(p => p.NodeId == startNodeId)
                ? Fail(pawnId, PutFailReason.StartNodeOccupied)
                : null;
        }

        private PutPawnResult Fail(byte pawnId, PutFailReason reason) => new(pawnId, reason);
    }
}