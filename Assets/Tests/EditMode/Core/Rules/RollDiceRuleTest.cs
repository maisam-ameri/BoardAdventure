using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;
using Core.Abstractions;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.EditMode.Core.Rules
{
    public class RollDiceRuleTest
    {
        private MatchState _matchState;
        private string _playerId;
        private IRandomNumberGenerator _numberGenerator;
        private RollDiceCommand _rollDiceCommand;
        private RollDiceRule _rollDiceRule;


        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.BuildMatchState();
            _numberGenerator = new RandomNumberGeneratorMock();
            _playerId = "player_1";
            _rollDiceCommand = new RollDiceCommand(_playerId);
            _rollDiceRule = new RollDiceRule(_numberGenerator);
        }

        [Test]
        public void Execute_Should_ReturnNotPlayerTurn_When_PlayerIsNotCurrentPlayer()
        {
            // Act
            _matchState.TurnState.CurrentPlayerId = "";
            var result = _rollDiceRule.Execute(_matchState, _rollDiceCommand);

            // Assert
            Assert.AreEqual(RollFailReason.NotPlayerTurn, result.FailReason);
        }

        [Test]
        public void Execute_Should_ReturnGeneratedStep_When_PlayerIsCurrentPlayer()
        {
            // Act
            _matchState.TurnState.CurrentPlayerId = _playerId;
            var result = _rollDiceRule.Execute(_matchState, _rollDiceCommand);

            // Assert
            Assert.AreEqual(RollFailReason.None, result.FailReason);
        }
    }
}