using System.Linq;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Turns
{
    public class TurnResolver
    {
        public void ResolveTurn(MatchState matchState)
        {
            if (!string.IsNullOrEmpty(matchState.WinnerId))
            {
                FinishMatch(matchState);
                return;
            }

            if (matchState.DiceState.Step == 6)
            {
                StartNextRoll(matchState);
                return;
            }

            SwitchTurn(matchState);
        }

        private void FinishMatch(MatchState state)
        {
            state.MatchInfo.Phase = MatchPhase.Finished;
        }

        private void StartNextRoll(MatchState state)
        {
            state.TurnState.Phase = TurnPhase.WaitingForRoll;
        }

        private void SwitchTurn(MatchState state)
        {
            var currentTurnOrder = state.PlayersState
                .First(p => p.PlayerId == state.TurnState.CurrentPlayerId).TurnOrder;

            var nextTurnOrder = 
                currentTurnOrder != state.PlayersState[^1].TurnOrder ? currentTurnOrder + 1 : 0;

            state.TurnState.CurrentPlayerId =
                state.PlayersState.First(p => p.TurnOrder == nextTurnOrder).PlayerId;
            
            state.TurnState.Phase = TurnPhase.WaitingForRoll;
        }
    }
}