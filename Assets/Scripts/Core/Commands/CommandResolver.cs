using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Commands
{
    public class MatchEngine : IMatchEngine
    {
        private readonly MatchState _matchState;
        private readonly MovePawnRule _movePawnRule;

        public MatchEngine(MatchState matchState, MovePawnRule movePawnRule)
        {
            _matchState = matchState;
            _movePawnRule = movePawnRule;
        }

        public void ResolveCommand(ICommand command, CommandContext context)
        {
            switch (command)
            {
                case MovePawnCommand moveCommand:
                    Handle(moveCommand, context);
                    // TODO
                    break;
            }
        }

        private void Handle(MovePawnCommand command, CommandContext context)
        {
            var result = _movePawnRule.Execute(_matchState, command, context);

            // TODO:
            // Apply result to MatchState
            // Publish signals
            // Send network messages
        }
    }
}