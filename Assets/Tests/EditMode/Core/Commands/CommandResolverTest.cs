using System.Linq;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.State;
using NUnit.Framework;

namespace Tests.EditMode.Core.Commands
{
    public class CommandResolverTest
    {
        private MatchState _matchState;

        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.CreateMatchState();
        }

        [Test]
        public void Resolve_Should_UpdatePawnPosition_When_MoveIsValid()
        {
            // Arrange
            byte pawnId = 1;
            int nodeId = 1;
            string playerId = "player_1";
            _matchState.BoardState.PawnsState.Add(
                new PawnState {NodeId = 1, PawnId = pawnId, OwnerPlayerId = playerId}
            );
            _matchState.TurnState.CurrentPlayerId = playerId;
            var command = new MovePawnCommand(pawnId);
            var resolver = new CommandResolver(_matchState);
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            resolver.Resolve(command, new CommandContext(playerId));

            // Assert
            Assert.AreNotEqual(nodeId, pawn.NodeId);
        }

        [Test]
        public void Resolve_Should_NotUpdatePawnPosition_When_MovePathIsInvalid()
        {
            // Arrange
            byte pawnId = 1;
            int nodeId = 5;
            string playerId = "player_1";

            _matchState.BoardState.PawnsState.Add(
                new PawnState {NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = playerId}
            );

            _matchState.TurnState.CurrentPlayerId = playerId;

            var command = new MovePawnCommand(pawnId);
            var resolver = new CommandResolver(_matchState);
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            resolver.Resolve(command, new CommandContext(playerId));

            // Assert
            Assert.AreEqual(nodeId, pawn.NodeId);
        }

        // TODO: After PathCalculator is implemented.

    }
}