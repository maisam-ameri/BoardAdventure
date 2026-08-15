using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace Tests.EditMode.Core.Consequences
{
    public class UpdateDiceStateConsequence
    {
        public void Execute(MatchState state, RollDiceResult rollDiceResult)
        {
            state.DiceState.Step = rollDiceResult.Step;
        }

    }
}