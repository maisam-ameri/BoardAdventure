using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using NUnit.Framework;

namespace Tests.EditMode.Core.Consequences
{
    public class CaptureConsequenceTest
    {
        private MatchState _matchState;

        [SetUp]
        public void Setup()
        {
            _matchState = TestHelper.BuildMatchState();
        }

        [Test]
        public void Execute_Should_CaptureEnemy_When_DestinationContainsEnemyPawn()
        {
            // Arrange
            var currentPlayerId = "player_1";
            var anotherPlayerId = "player_2";
            byte destinationNodeId = 1;
            byte enemyPawnId = 2;

            _matchState.TurnState.CurrentPlayerId = currentPlayerId;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = destinationNodeId, PawnId = 1, OwnerPlayerId = currentPlayerId},
                new PawnState {NodeId = destinationNodeId, PawnId = enemyPawnId, OwnerPlayerId = anotherPlayerId}
            };
            var movePawnResult = new MovePawnResult(1, MoveFailReason.None, new[] {destinationNodeId});
            var captureConsequence = new CaptureConsequence();

            // Acts
            captureConsequence.Execute(_matchState, movePawnResult);
            var enemyPawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == enemyPawnId);

            // Assert
            Assert.AreEqual(PawnLocationState.InBase, enemyPawn.PawnLocationState);
        }

        [Test]
        public void Execute_Should_NotCapture_When_DestinationHasNoEnemyPawn()
        {
            // Arrange
            var currentPlayerId = "player_1";
            var anotherPlayerId = "player_2";
            byte destinationNodeId = 1;
            byte enemyNodeId = 2;
            byte enemyPawnId = 2;


            _matchState.TurnState.CurrentPlayerId = currentPlayerId;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = destinationNodeId, PawnId = 1, OwnerPlayerId = currentPlayerId},
                new PawnState {NodeId = enemyNodeId, PawnId = enemyPawnId, OwnerPlayerId = anotherPlayerId}
            };
            var movePawnResult = new MovePawnResult(1, MoveFailReason.None, new[] {destinationNodeId});
            var captureConsequence = new CaptureConsequence();

            // Acts
            var enemyPawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == enemyPawnId);
            var enemyOriginalState = enemyPawn.PawnLocationState;
            captureConsequence.Execute(_matchState, movePawnResult);

            // Assert
            Assert.AreEqual(enemyOriginalState, enemyPawn.PawnLocationState);
        }
    }
}