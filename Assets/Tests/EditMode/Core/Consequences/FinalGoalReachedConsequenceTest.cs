using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using Core.Board;
using NUnit.Framework;

namespace Tests.EditMode.Core.Consequences
{
    public class FinalGoalReachedConsequenceTest
    {
        private MatchState _matchState;
        private IBoardDefinition _boardDefinition;


        [SetUp]
        public void Setup()
        {
            _matchState = TestHelper.CreateMatchState();
            _boardDefinition = new FakeBoardDefinition {IsFinalGoal = true};
        }

        [Test]
        public void Execute_Should_SetPawnLocationStateToInFinalGoal_When_PawnReachedFinalGoal()
        {
            // Arrange
            byte pawnId = 1;
            var nodeId = 1;
            var ownerPlayerId = "player_1";
            var factionType = FactionType.Blue;
            var locationState = PawnLocationState.OnBoard;

            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = ownerPlayerId, FactionType = factionType,
                    PawnLocationState = locationState
                },
            };

            var finalGoalReachedConsequence = new FinalGoalReachedConsequence(_boardDefinition);
            var movePawnResult = new MovePawnResult(pawnId, MoveFailReason.None, new[] {1});

            // Act
            finalGoalReachedConsequence.Execute(_matchState, movePawnResult);
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Assert
            Assert.AreEqual(PawnLocationState.InFinalGoal, pawn.PawnLocationState);
        }
    }
}