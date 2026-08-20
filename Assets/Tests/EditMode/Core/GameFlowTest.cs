using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.State;
using BoardAdventures.Core.Turns;
using Core.Board;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.EditMode.Core
{
    public class GameFlowTest
    {
        private MatchState _matchState;
        private TurnResolver _turnResolver;
        private CommandResolver _commandResolver;
        private SelectPawnResolver _selectPawnResolver;
        private string _firstPlayerId;
        private string _secondPlayerId;

        [SetUp]
        public void SetUp()
        {
            _firstPlayerId = "player_1";
            _secondPlayerId = "player_2";
            byte pawnId = 1;
            int nodeId = 1;

            _matchState = TestHelper.CreateMatchState();
            var boardDefinition = new FakeBoardDefinition();
            var randomNumberGenerator = new RandomNumberGeneratorMock();
            var moveConsequences = new List<IMoveConsequence>
            {
                new UpdatePawnPositionConsequence(),
                new CaptureConsequence(),
                new FinalGoalReachedConsequence(boardDefinition),
                new WinConsequence(),
            };

            _commandResolver = new CommandResolver(_matchState, moveConsequences, randomNumberGenerator);
            _turnResolver = new TurnResolver();
            _selectPawnResolver = new SelectPawnResolver();

            _matchState.TurnState.CurrentPlayerId = _firstPlayerId;
            _matchState.PlayersState = new List<PlayerState>
            {
                new() {PlayerId = _firstPlayerId, TurnOrder = 0},
                new() {PlayerId = _secondPlayerId, TurnOrder = 1}
            };
            _matchState.BoardState.PawnsState.Add(
                new PawnState
                {
                    NodeId = nodeId, PawnId = pawnId, OwnerPlayerId = _firstPlayerId,
                    PawnLocationState = PawnLocationState.OnBoard
                }
            );
        }

        [Test]
        public void Resolve_Should_PlayerTurn_From_Roll_To_Move()
        {
            // Arrange
            byte pawnId = 1;
            var exceptedNodeIdAfterMove = 5;
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);
            var context = new CommandContext(_firstPlayerId);
            var rollDiceCommand = new RollDiceCommand(_firstPlayerId);
            var selectPawnCommand = new SelectPawnCommand(pawnId);

            // Act
            _commandResolver.Resolve(rollDiceCommand, context);
            var afterSelectPawnCommand = _selectPawnResolver.Resolve(_matchState, selectPawnCommand, context);
            _commandResolver.Resolve(afterSelectPawnCommand, context);

            // Assert
            Assert.AreEqual(exceptedNodeIdAfterMove, pawn.NodeId);
        }

        [Test]
        public void Resolve_Should_PlayerTurn_From_Roll_To_SwitchTurn()
        {
            // Arrange
            byte pawnId = 1;
            var nextPlayerId = _secondPlayerId;
            var context = new CommandContext(_firstPlayerId);
            var selectPawnCommand = new SelectPawnCommand(pawnId);

            // Act
            var afterSelectPawnCommand = _selectPawnResolver.Resolve(_matchState, selectPawnCommand, context);
            _matchState.DiceState.Step = 4;
            _commandResolver.Resolve(afterSelectPawnCommand, context);
            _turnResolver.Resolve(_matchState);

            // Assert
            Assert.AreEqual(nextPlayerId, _matchState.TurnState.CurrentPlayerId);
        }
        
        [Test]
        public void Resolve_Should_PlayerTurn_From_Roll_To_KeepTurn()
        {
            // Arrange
            byte pawnId = 1;
            var currentPlayerId = _firstPlayerId;
            var context = new CommandContext(_firstPlayerId);
            var selectPawnCommand = new SelectPawnCommand(pawnId);

            // Act
            var afterSelectPawnCommand = _selectPawnResolver.Resolve(_matchState, selectPawnCommand, context);
            _matchState.DiceState.Step = 6;
            _commandResolver.Resolve(afterSelectPawnCommand, context);
            _turnResolver.Resolve(_matchState);

            // Assert
            Assert.AreEqual(currentPlayerId, _matchState.TurnState.CurrentPlayerId);
        }
        
        [Test]
        public void Resolve_Should_PlayerTurn_From_Roll_To_FinishMatch()
        {
            // Arrange
            byte pawnId = 1;
            var pawn = _matchState.BoardState.PawnsState.First(p => p.PawnId == pawnId);
            var context = new CommandContext(_firstPlayerId);
            var selectPawnCommand = new SelectPawnCommand(pawnId);

            // Act
            var afterSelectPawnCommand = _selectPawnResolver.Resolve(_matchState, selectPawnCommand, context);
            pawn.PawnLocationState = PawnLocationState.InFinalGoal;
            _commandResolver.Resolve(afterSelectPawnCommand, context);
            _turnResolver.Resolve(_matchState);

            // Assert
            Assert.AreEqual(MatchPhase.Finished, _matchState.MatchInfo.Phase);
        }
    }
}