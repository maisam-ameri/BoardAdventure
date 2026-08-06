using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.State;
using BoardAdventures.Core.Turns;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Core.Turns
{
    public class TurnResolverTest
    {
        private MatchState _matchState;
        private TurnResolver _turnResolver;
        private string _firstPlayerId;
        private string _secondPlayerId;

        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.CreateMatchState();
            _turnResolver = new TurnResolver();
            _firstPlayerId = "player_1";
            _secondPlayerId = "player_2";

            _matchState.TurnState.CurrentPlayerId = _firstPlayerId;
            _matchState.PlayersState = new List<PlayerState>
            {
                new() {PlayerId = _firstPlayerId, TurnOrder = 0},
                new() {PlayerId = _secondPlayerId, TurnOrder = 1}
            };
        }

        [Test]
        public void ResolveTurn_Should_SetMatchPhaseToFinished_When_WinnerExists()
        {
            // Act
            _matchState.WinnerId = _matchState.TurnState.CurrentPlayerId;
            _turnResolver.ResolveTurn(_matchState);

            // Assert
            Assert.AreEqual(MatchPhase.Finished, _matchState.MatchInfo.Phase);
            Assert.AreEqual(_firstPlayerId, _matchState.TurnState.CurrentPlayerId);
        }

        [Test]
        public void ResolveTurn_Should_KeepCurrentPlayer_When_DiceStepIsSix()
        {
            // Act
            _matchState.DiceState.Step = 6;
            _turnResolver.ResolveTurn(_matchState);

            // Assert
            Assert.AreEqual(TurnPhase.WaitingForRoll, _matchState.TurnState.Phase);
            Assert.AreEqual(_firstPlayerId, _matchState.TurnState.CurrentPlayerId);
        }

        [Test]
        public void ResolveTurn_Should_SwitchToNextPlayer_When_NoWinnerAndDiceStepIsNotSix()
        {
            // Act
            _matchState.WinnerId = string.Empty;
            _matchState.DiceState.Step = 4;
            _turnResolver.ResolveTurn(_matchState);


            // Assert
            Assert.AreEqual(_secondPlayerId, _matchState.TurnState.CurrentPlayerId);
            Assert.AreEqual(TurnPhase.WaitingForRoll, _matchState.TurnState.Phase);
        }

        [Test]
        public void ResolveTurn_Should_WrapToFirstPlayer_When_CurrentPlayerIsLastInTurnOrder()
        {
            // Act
            _matchState.WinnerId = string.Empty;
            _matchState.DiceState.Step = 4;
            _matchState.TurnState.CurrentPlayerId = _secondPlayerId;
            _turnResolver.ResolveTurn(_matchState);


            // Assert
            Assert.AreEqual(_firstPlayerId, _matchState.TurnState.CurrentPlayerId);
            Assert.AreEqual(TurnPhase.WaitingForRoll, _matchState.TurnState.Phase);
        }
    }
}