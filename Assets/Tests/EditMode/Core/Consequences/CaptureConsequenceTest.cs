using System.Collections.Generic;
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
            _matchState = TestHelper.CreateMatchState();
        }

        [Test]
        public void Execute_Should_CaptureEnemy_When_DestinationContainsEnemyPawn()
        {
            // Arrange
            var currentPlayerId = "player_1";            
            var anotherPlayerId = "player_2";
            var destinationNodeId  = 1;
            
            _matchState.TurnState.CurrentPlayerId = currentPlayerId;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = destinationNodeId, PawnId = 1, OwnerPlayerId = currentPlayerId},
                new PawnState {NodeId = destinationNodeId, PawnId = 2, OwnerPlayerId = anotherPlayerId}
            };
            var movePawnResult = new MovePawnResult(1, MoveFailReason.None, new[] {destinationNodeId});
            var captureConsequence = new CaptureConsequence();
            
            // Acts
            var captureResult = captureConsequence.Execute(_matchState, movePawnResult);
            
            // Assert
            Assert.IsTrue(captureResult);
        } 
        
        [Test]
        public void Execute_Should_NotCaptureEnemy_When_ThereIsNoEnemyOnTheDestination()
        {
            // Arrange
            var currentPlayerId = "player_1";            
            var anotherPlayerId = "player_2";
            var destinationNodeId  = 1;
            var enemyNodeId = 2;
            
            _matchState.TurnState.CurrentPlayerId = currentPlayerId;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = destinationNodeId, PawnId = 1, OwnerPlayerId = currentPlayerId},
                new PawnState {NodeId = enemyNodeId, PawnId = 2, OwnerPlayerId = anotherPlayerId}
            };
            var movePawnResult = new MovePawnResult(1, MoveFailReason.None, new[] {destinationNodeId});
            var captureConsequence = new CaptureConsequence();
            
            // Acts
            var captureResult = captureConsequence.Execute(_matchState, movePawnResult);
            
            // Assert
            Assert.IsFalse(captureResult);
            // TODO: return enemy to the base
            
        }
    }
}