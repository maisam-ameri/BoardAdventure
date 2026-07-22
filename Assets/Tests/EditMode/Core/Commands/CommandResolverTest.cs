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
            string playerId = "player_1";
            _matchState.BoardState.PawnsState.Add(
                new PawnState {NodeId = 1, PawnId = pawnId, OwnerPlayerId = playerId}
            );
            _matchState.TurnState.CurrentPlayerId = playerId;
            var command = new MovePawnCommand(pawnId);
            var resolver = new CommandResolver(_matchState);

            // Act
            resolver.Resolve(command, new CommandContext(playerId));

            // Assert
            Assert.AreEqual(5, _matchState.BoardState.PawnsState
                .First(p => p.PawnId == pawnId).NodeId);
        }

        // TODO:
        // Add resolver test for invalid path
        // after PathCalculator is implemented.
    }
}