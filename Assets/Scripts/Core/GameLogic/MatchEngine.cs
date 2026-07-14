using BoardAdventures.Abstractions;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Rules;
using Core.State;

namespace BoardAdventures.Core.GameLogic
{
    public class MatchEngine: IMatchEngine
    {
        private readonly MatchState _matchState;
        private readonly MovePawnRule _movePawnRule;

        public MatchEngine(MatchState matchState, MovePawnRule movePawnRule)
        {
            _matchState = matchState;
            _movePawnRule = movePawnRule;
        }

        public void HandleCommand(ICommand command)
        {
            switch (command)
            {
                case MovePawnCommand moveCommand:
                    Handle(moveCommand);
                    // TODO
                break;
            }
        }

        private void Handle(MovePawnCommand moveCommand)
        {
            var result =_movePawnRule.Execute(_matchState, moveCommand);
            
            // TODO:
            // Apply result to MatchState
            // Publish signals
            // Send network messages
        }
    }
}