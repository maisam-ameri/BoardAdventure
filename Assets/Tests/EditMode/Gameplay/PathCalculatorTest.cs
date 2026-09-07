using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.State;
using NUnit.Framework;

namespace Tests.EditMode.Gameplay
{
    public class PathCalculatorTest
    {
        private MatchState _matchState;
        private PathCalculator _pathCalculator;


        [SetUp]
        public void Setup()
        {
            _matchState = TestHelper.BuildMatchState();
            _pathCalculator = new PathCalculator(TestHelper.BuildBoardDefinition());
        }

        [Test]
        public void Calculate_Should_ReturnLinearPath_When_PawnMoveOnNormalPath()
        {
            // Arrange
            var pawnId = 1;
            byte? step = 2;
            _matchState.DiceState.Step = step;
            _matchState.BoardState.PawnsState = new List<PawnState>
                {new() {NodeId = 1, PawnId = 1, FactionType = FactionType.Blue}};

            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            var path = _pathCalculator.Calculate(pawn, step);

            // Assert
            Assert.AreEqual(new[] {2, 3}, path);
        }

        [Test]
        public void Calculate_Should_ReturnPath_When_PawnMoveOnGoalNodes()
        {
            // Arrange
            var pawnId = 1;
            byte? step = 5;
            _matchState.DiceState.Step = step;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new() {NodeId = 0, PawnId = 1, FactionType = FactionType.Blue}
            };

            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            var path = _pathCalculator.Calculate(pawn, step);

            // Assert
            CollectionAssert.AreEqual(new[] {1,2, 3, 4, 5}, path);
        }

        [Test]
        public void Calculate_Should_ReturnNull_When_NoValidPath()
        {
            // Arrange
            var pawnId = 1;
            byte? step = 6;
            _matchState.DiceState.Step = step;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new() {NodeId = 1, PawnId = 1, FactionType = FactionType.Blue}
            };

            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            var path = _pathCalculator.Calculate(pawn, step);

            // Assert
            Assert.IsNull(path);
        }

        [Test]
        public void Calculate_Should_ContinueAlongPath_When_ReachingOpponentGate()
        {
            // Arrange
            var pawnId = 1;
            byte? step = 4;
            _matchState.DiceState.Step = step;
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new() {NodeId = 0, PawnId = 1, FactionType = FactionType.Red}
            };

            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);

            // Act
            var path = _pathCalculator.Calculate(pawn, step);

            // Assert
            CollectionAssert.AreEqual(new[] {1, 2, 3, 7}, path);
        }
    }
}