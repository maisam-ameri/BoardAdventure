using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.State;
using NUnit.Framework;

namespace Tests.EditMode.Core.Consequences
{
    public class ExtraTurnConsequenceTest
    {
        private MatchState _matchState;

        
        [SetUp]
        public void SetUp()
        {
            _matchState = TestHelper.CreateMatchState();
        }
        
        [Test]
        public void Execute_Should_GrantExtraTurn_When_DiceStepIsSix()
        {
            // Arrange
            var extraTurnConsequence = new ExtraTurnConsequence();
            
            // Act
            _matchState.DiceState.Step = 6;
            extraTurnConsequence.Execute(_matchState,null);
            // Assert
            Assert.IsTrue(_matchState.TurnState.ExtraTurnGranted);
            
        }
        
        [Test]
        public void Execute_Should_NotGrantExtraTurn_When_DiceStepIsNotSix()
        {
            // Arrange
            var extraTurnConsequence = new ExtraTurnConsequence();
            
            // Act
            _matchState.DiceState.Step = 1;
            extraTurnConsequence.Execute(_matchState,null);
            // Assert
            Assert.IsFalse(_matchState.TurnState.ExtraTurnGranted);
            
        }
        
      
    }
}