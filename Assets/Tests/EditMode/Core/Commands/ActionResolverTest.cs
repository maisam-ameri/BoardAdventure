using BoardAdventures.Core.Commands;
using BoardAdventures.Core.State;
using Core.Board;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Core.Commands
{
    public class ActionResolverTest
    {
        private MatchState _matchState;
        private ActionResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.CreateMatchState();
            _resolver = new ActionResolver();
        }

        [Test]
        public void Resolve_Should_ReturnMoveCommand_WhenPlayerSelectOnBoardPawn()
        {
            // Arrange
            byte pawnId = 1;
            _matchState.BoardState.PawnsState.Add(
                new PawnState {PawnId = pawnId, PawnLocationState = PawnLocationState.OnBoard}
            );

            // Act
            var command = _resolver.Resolve(_matchState, pawnId);

            // Assert
            Assert.IsInstanceOf<MovePawnCommand>(command);
            Assert.AreEqual(pawnId,((MovePawnCommand)command).PawnId);
        }
    }
}