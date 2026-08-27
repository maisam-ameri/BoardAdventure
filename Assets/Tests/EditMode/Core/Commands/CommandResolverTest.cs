using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.State;
using Core.Board;
using Core.Consequences;
using NUnit.Framework;
using Tests.Mocks;

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
            var nodes = new List<INode>();
            var boardDefinition = new FakeBoardDefinition(nodes);
            var randomNumberGenerator = new RandomNumberGeneratorMock();
            var putConsequence = new UpdatePawnPositionAfterPutConsequence();
            var moveConsequences = new List<IMoveConsequence>
            {
                new UpdatePawnPositionAfterMoveConsequence(),
                new CaptureConsequence(),
                new FinalGoalReachedConsequence(boardDefinition),
                new WinConsequence(),
            };

            _resolver = new CommandResolver(_matchState, moveConsequences, putConsequence
                , randomNumberGenerator, boardDefinition);
        }

        [Test]
        public void Resolve_Should_UpdatePawnPosition_When_MoveIsValid()
        {
            // Arrange
            byte pawnId = 1;
            byte nodeId = 1;
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
            Assert.AreNotEqual(nodeId, pawn.NodeId);
        }

        [Test]
        public void Resolve_Should_NotUpdatePawnPosition_When_MovePathIsInvalid()
        {
            // Arrange
            byte pawnId = 1;
            byte nodeId = 5;
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