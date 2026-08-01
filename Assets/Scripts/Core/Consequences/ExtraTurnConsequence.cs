using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Consequences
{
    public class ExtraTurnConsequence: IMoveConsequence
    {
        public int Order { get; } = ConsequenceOrder.ExtraTurn;
        public void Execute(MatchState matchState, MovePawnResult movePawnResult)
        {
            matchState.TurnState.ExtraTurnGranted = matchState.DiceState.Step == 6;
        }
    }
}