using System.Collections.Generic;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using Core.Board;
using NUnit.Framework;

namespace Tests.EditMode.Core.Consequences
{
    public class WinConsequenceTest
    {
        
        private MatchState _matchState;
        private IBoardDefinition _boardDefinition;
        
        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.CreateMatchState();
            _boardDefinition = new FakeBoardDefinition {IsFinalGoal = true};
        }

        [Test]
        public void Execute_Should_SetWinner_When_AllPlayerPawnsReachedFinalGoal()
        {
            // Arrange
            byte pawnId = 1;
            byte nodeId = 1;
            var ownerPlayerId = "player_1";
            var factionType = FactionType.Blue;
            var locationState = PawnLocationState.InFinalGoal;

            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = ownerPlayerId, FactionType = factionType,
                    PawnLocationState = locationState
                },
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = ownerPlayerId, FactionType = factionType,
                    PawnLocationState = locationState
                },
            };

            var winConsequence = new WinConsequence();
            var movePawnResult = new MovePawnResult(pawnId, MoveFailReason.None, new byte[] {1});
            // Act
            winConsequence.Execute(_matchState, movePawnResult);
            
            // Assert
            Assert.AreEqual(ownerPlayerId,_matchState.WinnerId);
        }   
        
        
        [Test]
        public void Execute_Should_NotSetWinner_When_NotAllPlayerPawnsReachedFinalGoal()
        {
            // Arrange
            byte pawnId = 1;
            byte nodeId = 1;
            var ownerPlayerId = "player_1";
            var factionType = FactionType.Blue;
            var locationState = PawnLocationState.InFinalGoal;

            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = ownerPlayerId, FactionType = factionType,
                    PawnLocationState = locationState
                },
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = ownerPlayerId, FactionType = factionType,
                    PawnLocationState = PawnLocationState.OnBoard
                },
            };

            var winConsequence = new WinConsequence();
            var movePawnResult = new MovePawnResult(pawnId, MoveFailReason.None, new byte[] {1});
            // Act
            winConsequence.Execute(_matchState, movePawnResult);
            
            // Assert
            Assert.IsNull(_matchState.WinnerId);
        }  
        
    }
}