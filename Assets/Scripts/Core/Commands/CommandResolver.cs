using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;
using Core.Abstractions;
using Tests.EditMode.Core.Consequences;

namespace BoardAdventures.Core.Commands
{
    public class CommandResolver : IMatchEngine
    {
        private readonly MatchState _matchState;
        private readonly IEnumerable<IMoveConsequence> _consequences;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public CommandResolver(MatchState matchState, IEnumerable<IMoveConsequence> consequences, IRandomNumberGenerator randomNumberGenerator)
        {
            _matchState = matchState;
            _consequences = consequences.OrderBy(c => c.Order);
            _randomNumberGenerator = randomNumberGenerator;
        }

        public void Resolve(ICommand command, CommandContext context)
        {
            switch (command)
            {
                case MovePawnCommand moveCommand:
                    MovePawn(moveCommand, new MovePawnRule(), context);
                    break;
                
                case RollDiceCommand rollDiceCommand:
                    RollDice(rollDiceCommand, new RollDiceRule(_randomNumberGenerator));
                    break;
            }
        }

        private void MovePawn(MovePawnCommand command, MovePawnRule rule, CommandContext context)
        {
            var movePawnResult = rule.Execute(_matchState, command, context);

            foreach (var consequence in _consequences)
            {
                consequence.Execute(_matchState,movePawnResult);
            }

            // TODO: Publish signals
            // TODO: Send network messages
        }
        
        private void RollDice(RollDiceCommand command, RollDiceRule rule)
        {
            var rollDiceResult = rule.Execute(_matchState, command);
            var diceStateConsequence = new UpdateDiceStateConsequence();

            if (rollDiceResult.FailReason == RollFailReason.None)
            {
                diceStateConsequence.Execute(_matchState, rollDiceResult);
            }
                

            // TODO: Publish signals
            // TODO: Send network messages
        }
    }
}