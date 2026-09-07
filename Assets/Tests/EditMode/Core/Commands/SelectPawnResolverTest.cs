using BoardAdventures.Core.Commands;
using BoardAdventures.Core.State;
using NUnit.Framework;

namespace Tests.EditMode.Core.Commands
{
    public class SelectPawnResolverTest
    {
        private MatchState _matchState;
        private SelectPawnResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.BuildMatchState();
            _resolver = new SelectPawnResolver();
        }

        [Test]
        public void Resolve_Should_ReturnMoveCommand_WhenPlayerSelectOnBoardPawn()
        {
            // Arrange
            var playerId = "player_1";
            byte pawnId = 1;
            var selectPawnCommand = new SelectPawnCommand(pawnId);
            var context = new CommandContext(playerId);
            _matchState.BoardState.PawnsState.Add(
                new PawnState {PawnId = pawnId, OwnerPlayerId = playerId, PawnLocationState = PawnLocationState.OnBoard}
            );

            // Act
            var command = _resolver.Resolve(_matchState, selectPawnCommand, context);

            // Assert
            Assert.IsInstanceOf<MovePawnCommand>(command);
            Assert.AreEqual(pawnId, ((MovePawnCommand) command).PawnId);
        }
    }
}