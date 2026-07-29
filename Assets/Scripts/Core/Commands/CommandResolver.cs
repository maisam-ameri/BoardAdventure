using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.Consequences;
using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Commands
{
    public class CommandResolver : IMatchEngine
    {
        private readonly MatchState _matchState;
        private readonly CaptureConsequence _captureConsequence;
        private readonly FinalGoalReachedConsequence _finalGoalReachedConsequence;
        private readonly IEnumerable<IMoveConsequence> _consequences;
        
        public CommandResolver(MatchState matchState, IEnumerable<IMoveConsequence> consequences)
        {
            _matchState = matchState;
            _consequences = consequences.OrderBy(c => c.Order);
        }

        public void Resolve(ICommand command, CommandContext context)
        {
            switch (command)
            {
                case MovePawnCommand moveCommand:
                    Handle(moveCommand, new MovePawnRule(), context);
                    break;
            }
        }

        private void Handle(MovePawnCommand command, MovePawnRule rule, CommandContext context)
        {
            var movePawnResult = rule.Execute(_matchState, command, context);
            
            _matchState.BoardState.PawnsState
                .First(p => p.PawnId == command.PawnId).NodeId = movePawnResult.Path.Last();

            foreach (var consequence in _consequences)
            {
                consequence.Execute(_matchState,movePawnResult);
            }
            
            // TODO: Check Win
            // TODO: Check Extra Turn

            // Publish signals
            // Send network messages
        }
    }
}