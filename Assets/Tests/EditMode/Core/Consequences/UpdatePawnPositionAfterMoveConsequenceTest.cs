using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using NUnit.Framework;

namespace Tests.EditMode.Core.Consequences
{
    public class UpdatePawnPositionAfterMoveConsequenceTest
    {
        private MatchState _matchState;


        [SetUp]
        public void Setup()
        {
            _matchState = TestHelper.BuildMatchState();
        }

        [Test]
        public void Execute_Should_UpdatePawnPosition_When_MoveIsValid()
        {
            // Arrange
            var currentPlayerId = "player_1";
            byte pawnId = 1;
            byte currentNodeId = 1;
            byte destinationNodeId = 2;

            _matchState.TurnState.CurrentPlayerId = currentPlayerId;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = currentNodeId, PawnId = pawnId, OwnerPlayerId = currentPlayerId},
            };

            var movePawnResult = new MovePawnResult(pawnId, MoveFailReason.None, new[] {destinationNodeId});
            var updatePawnPositionConsequence = new UpdatePawnPositionAfterMoveConsequence();

            // Act
            updatePawnPositionConsequence.Execute(_matchState, movePawnResult);
            
            // Assert
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);
            Assert.AreEqual(destinationNodeId , pawn.NodeId);
        }
    }
}