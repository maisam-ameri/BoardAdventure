using System.Collections.Generic;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Rules;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using NUnit.Framework;

namespace Tests.EditMode.Core.Rules
{
    public class MovePawnRuleTest
    {
        private CommandContext _commandContext;
        private MovePawnRule _rule;
        private MatchState _matchState;

        [SetUp]
        public void Setup()
        {
            _matchState = TestHelper.CreateMatchState();
            _commandContext = new CommandContext("player_1");
            _rule = new MovePawnRule();
        }

        [Test]
        public void Execute_Should_ReturnPawnNotFound_When_PawnDoesNotExist()
        {
            // Arrange
            var command = new MovePawnCommand(1);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>();
            _matchState.TurnState.CurrentPlayerId = _commandContext.PlayerId;

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreEqual(MoveFailReason.PawnNotFound, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnPawn_When_PawnExists()
        {
            // Arrange
            var command = new MovePawnCommand(1);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState> {new PawnState {NodeId = 0, PawnId = 1}};
            _matchState.TurnState.CurrentPlayerId = _commandContext.PlayerId;

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreNotEqual(MoveFailReason.PawnNotFound, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnNotPlayersTurn_When_PlayerTriesToMoveOutOfTurn()
        {
            // Arrange
            var command = new MovePawnCommand(1);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState> {new PawnState {NodeId = 0, PawnId = 1}};
            _matchState.TurnState.CurrentPlayerId = "";

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreEqual(MoveFailReason.NotPlayersTurn, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnPawnDoesNotBelongToPlayer_When_PlayerSelectsOpponentPawn()
        {
            // Arrange
            var command = new MovePawnCommand(1);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = 0, PawnId = 1, OwnerPlayerId = ""}
            };
            _matchState.TurnState.CurrentPlayerId = "player_1";


            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreEqual(MoveFailReason.PawnDoesNotBelongToPlayer, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnContinue_When_PlayerSelectsOwnPawn()
        {
            // Arrange
            var command = new MovePawnCommand(1);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = 0, PawnId = 1, OwnerPlayerId = "player_1"}
            };
            _matchState.TurnState.CurrentPlayerId = "player_1";

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreNotEqual(MoveFailReason.PawnDoesNotBelongToPlayer, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnPath_When_ValidPathExists()
        {
            // Arrange
            var command = new MovePawnCommand(1);
            var pawnState = new List<PawnState>
            {
                new PawnState {NodeId = 0, PawnId = 1, OwnerPlayerId = "player_1"}
            };

            // Act
            _matchState.BoardState.PawnsState = pawnState;
            _matchState.TurnState.CurrentPlayerId = "player_1";
            _matchState.DiceState.Step = 5;

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            CollectionAssert.AreEqual(
                new[] {1, 2, 3, 4, 5}, result.Path
            );
        }

        [Test]
        public void Execute_Should_ReturnDestinationOccupied_When_LastNodeContainsFriendlyPawn()
        {
            // Arrange
            byte OccupiedNodeId = 5;
            var command = new MovePawnCommand(1);
            var pawnState = new List<PawnState>
            {
                new PawnState {NodeId = 0, PawnId = 1, OwnerPlayerId = "player_1"},
                new PawnState {NodeId = OccupiedNodeId, PawnId = 2, OwnerPlayerId = "player_1"},
                
            };

            // Act
            _matchState.BoardState.PawnsState = pawnState;
            _matchState.TurnState.CurrentPlayerId = "player_1";
            _matchState.DiceState.Step = 5;

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreEqual( MoveFailReason.DestinationOccupied, result.FailReason);
        }
        
        
    }
}