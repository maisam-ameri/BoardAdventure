using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;
using Core.Abstractions;

namespace BoardAdventures.Core.Rules
{
    public class RollDiceRule
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public RollDiceRule(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public RollDiceResult Execute(MatchState matchState, RollDiceCommand command)
        {
            var result = ValidatePlayersTurn(matchState, command.PlayerId);

            return result;
        }

        private RollDiceResult ValidatePlayersTurn(MatchState state, string playerId)
        {
            if (state.TurnState.CurrentPlayerId != playerId)
                return new RollDiceResult(0, RollFailReason.NotPlayerTurn);

            var step = _randomNumberGenerator.GenerateNumber();

            return new RollDiceResult(step, RollFailReason.None);
        }
    }
}