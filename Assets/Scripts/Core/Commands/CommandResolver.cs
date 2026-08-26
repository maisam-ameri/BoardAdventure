using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Results;
using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;
using Core.Abstractions;
using Core.Consequences;
using Tests.EditMode.Core.Consequences;

namespace BoardAdventures.Core.Commands
{
    public class CommandResolver : IMatchEngine
    {
        private readonly MatchState _matchState;
        private readonly IEnumerable<IMoveConsequence> _moveConsequences;
        private readonly UpdatePawnPositionAfterPutConsequence _putConsequences;
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly IBoardDefinition _boardDefinition;

        public CommandResolver(MatchState matchState, IEnumerable<IMoveConsequence> consequences,
            IRandomNumberGenerator randomNumberGenerator, IBoardDefinition boardDefinition)
        {
            _matchState = matchState;
            _moveConsequences = consequences.OrderBy(c => c.Order);
            _randomNumberGenerator = randomNumberGenerator;
            _boardDefinition = boardDefinition;
        }

        public void Resolve(ICommand command, CommandContext context)
        {
            switch (command)
            {
                case MovePawnCommand moveCommand:
                    MovePawn(moveCommand, new MovePawnRule(), context);
                    break;
                
                case PutPawnCommand putCommand:
                    PutPawn(putCommand, new PutPawnRule(_boardDefinition), context);
                    break;

                case RollDiceCommand rollDiceCommand:
                    RollDice(rollDiceCommand, new RollDiceRule(_randomNumberGenerator));
                    break;
            }
        }

        private void MovePawn(MovePawnCommand command, MovePawnRule rule, CommandContext context)
        {
            var movePawnResult = rule.Execute(_matchState, command, context);

            foreach (var consequence in _moveConsequences)
            {
                consequence.Execute(_matchState, movePawnResult);
            }

            // TODO: Publish signals
            // TODO: Send network messages
        }

        private void PutPawn(PutPawnCommand command, PutPawnRule rule, CommandContext context)
        {
            var putPawnResult = rule.Execute(_matchState, command, context);
            _putConsequences.Execute(_matchState, putPawnResult);


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