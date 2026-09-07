using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using Core.Consequences;
using NUnit.Framework;

namespace Tests.EditMode.Core.Consequences
{
    public class UpdatePawnPositionAfterPutConsequenceTest
    {
        private MatchState _matchState;


        [SetUp]
        public void Setup()
        {
            _matchState = TestHelper.BuildMatchState();
        }

        [Test]
        public void Execute_Should_UpdatePawnPosition_When_PutPawn()
        {
            // Arrange
            var currentPlayerId = "player_1";
            byte pawnId = 1;
            byte currentNodeId = 0;
            byte destinationNodeId = 1;

            _matchState.TurnState.CurrentPlayerId = currentPlayerId;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = currentNodeId, PawnId = pawnId, OwnerPlayerId = currentPlayerId},
            };

            var putPawnResult = new PutPawnResult(pawnId, PutFailReason.None, destinationNodeId);
            var updatePawnPositionAfterPutConsequence = new UpdatePawnPositionAfterPutConsequence();

            // Act
            updatePawnPositionAfterPutConsequence.Execute(_matchState, putPawnResult);

            // Assert
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);
            Assert.AreEqual(destinationNodeId, pawn.NodeId);
        }
    }
}