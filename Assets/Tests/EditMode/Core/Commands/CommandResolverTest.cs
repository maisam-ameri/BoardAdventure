using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.State;
using Core.Board;
using NUnit.Framework;

namespace Tests.EditMode.Core.Commands
{
    public class CommandResolverTest
    {
        private MatchState _matchState;
        private CommandResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.CreateMatchState();
            var boardDefinition = new FakeBoardDefinition();
            var consequences = new List<IMoveConsequence>
            {
                new CaptureConsequence(),
                new FinalGoalReachedConsequence(boardDefinition)
            };
            
            _resolver = new CommandResolver(_matchState, consequences);

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
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            _resolver.Resolve(command, new CommandContext(playerId));

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
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            _resolver.Resolve(command, new CommandContext(playerId));

            // Assert
            Assert.AreEqual(nodeId, pawn.NodeId);
        }

        // TODO: After PathCalculator is implemented.

    }
}