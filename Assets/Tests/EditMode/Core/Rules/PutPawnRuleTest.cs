using System.Collections.Generic;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;
using Core.Board;
using NUnit.Framework;

namespace Tests.EditMode.Core.Rules
{
    public class PutPawnRuleTest
    {
        private CommandContext _commandContext;
        private PutPawnRule _rule;
        private MatchState _matchState;
        private FakeBoardDefinition _boardDefinition;

        [SetUp]
        public void Setup()
        {
            _matchState = TestHelper.CreateMatchState();
            _commandContext = new CommandContext("player_1");
            _boardDefinition = new FakeBoardDefinition(new List<INode>()
                {
                    new Node(
                        nodeId: 0,
                        prevNodeId: null,
                        nextNodeId: null,
                        nodeType: NodeType.Base,
                        factionType: FactionType.Blue
                    ),
                    new Node(
                        nodeId: 1,
                        prevNodeId: null,
                        nextNodeId: 2,
                        nodeType: NodeType.Start,
                        factionType: FactionType.Blue
                    )
                }
            );
            _rule = new PutPawnRule(_boardDefinition);
        }

        [Test]
        public void Execute_Should_ReturnPawnNotFound_When_PawnDoesNotExist()
        {
            // Arrange
            byte pawnId = 1;
            var command = new PutPawnCommand(pawnId);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>();
            _matchState.TurnState.CurrentPlayerId = _commandContext.PlayerId;

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreEqual(PutFailReason.PawnNotFound, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnPawn_When_PawnExists()
        {
            // Arrange
            byte pawnId = 1;
            var command = new PutPawnCommand(pawnId);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState> {new PawnState {NodeId = 0, PawnId = 1}};
            _matchState.TurnState.CurrentPlayerId = _commandContext.PlayerId;

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreNotEqual(PutFailReason.PawnNotFound, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnNotPlayersTurn_When_PlayerTriesToPutOutOfTurn()
        {
            // Arrange
            byte pawnId = 1;
            var command = new PutPawnCommand(pawnId);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState> {new PawnState {NodeId = 0, PawnId = 1}};
            _matchState.TurnState.CurrentPlayerId = "";

            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreEqual(PutFailReason.NotPlayersTurn, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnPawnDoesNotBelongToPlayer_When_PlayerSelectsOpponentPawn()
        {
            // Arrange
            byte pawnId = 1;
            var command = new PutPawnCommand(pawnId);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState {NodeId = 0, PawnId = 1, OwnerPlayerId = "", FactionType = FactionType.Blue}
            };
            _matchState.TurnState.CurrentPlayerId = "player_1";


            var result = _rule.Execute(_matchState, command, _commandContext);

            // Assert
            Assert.AreEqual(PutFailReason.PawnDoesNotBelongToPlayer, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnContinue_When_PlayerSelectsOwnPawn()
        {
            // Arrange
            byte pawnId = 1;
            var command = new PutPawnCommand(pawnId);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState
                    {NodeId = 0, PawnId = 1, OwnerPlayerId = _commandContext.PlayerId, FactionType = FactionType.Blue}
            };
            _matchState.TurnState.CurrentPlayerId = "player_1";

            var result = _rule.Execute(_matchState, command, _commandContext);
            // Assert
            Assert.AreNotEqual(PutFailReason.PawnDoesNotBelongToPlayer, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnFailure_WhenStartNodeIsOccupied()
        {
            // Arrange
            byte pawnId = 0;
            byte nodeId = 1;
            var command = new PutPawnCommand(pawnId);

            _boardDefinition = new FakeBoardDefinition(new List<INode>()
                {
                    new Node(
                        nodeId: 1,
                        prevNodeId: null,
                        nextNodeId: 2,
                        nodeType: NodeType.Start,
                        factionType: FactionType.Blue
                    )
                }
            );
            _rule = new PutPawnRule(_boardDefinition);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = _commandContext.PlayerId,
                    FactionType = FactionType.Blue
                }
            };
            _matchState.TurnState.CurrentPlayerId = "player_1";

            var result = _rule.Execute(_matchState, command, _commandContext);
            // Assert
            Assert.AreEqual(PutFailReason.StartNodeOccupied, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnSuccess_WhenPutPawnIsValid()
        {
            // Arrange
            byte pawnId = 0;
            byte nodeId = 0;
            var command = new PutPawnCommand(pawnId);

            _boardDefinition = new FakeBoardDefinition(new List<INode>()
                {
                    new Node(
                        nodeId: 1,
                        prevNodeId: null,
                        nextNodeId: 2,
                        nodeType: NodeType.Start,
                        factionType: FactionType.Blue
                    )
                }
            );
            _rule = new PutPawnRule(_boardDefinition);

            // Act
            _matchState.BoardState.PawnsState = new List<PawnState>
            {
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = _commandContext.PlayerId,
                    FactionType = FactionType.Blue
                }
            };
            _matchState.TurnState.CurrentPlayerId = "player_1";

            var result = _rule.Execute(_matchState, command, _commandContext);
            // Assert
            Assert.AreEqual(PutFailReason.None, result.FailReason);
            Assert.IsNotNull(result.NodeId);
        }
    }
}