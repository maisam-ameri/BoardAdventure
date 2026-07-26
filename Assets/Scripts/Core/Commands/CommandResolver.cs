using System.Linq;
using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;
using UnityEngine;

namespace BoardAdventures.Core.Commands
{
    public class CommandResolver : IMatchEngine
    {
        private readonly MatchState _matchState;

        public CommandResolver(MatchState matchState)
        {
            _matchState = matchState;
        }

        public void Resolve(ICommand command, CommandContext context)
        {
            switch (command)
            {
                case MovePawnCommand moveCommand:
                    Handle(moveCommand, new MovePawnRule(), context);
                    // TODO
                    break;
            }
        }

        private void Handle(MovePawnCommand command, MovePawnRule rule, CommandContext context)
        {
            var result = rule.Execute(_matchState, command, context);
 
            _matchState.BoardState.PawnsState
                .First(p => p.PawnId == command.PawnId).NodeId = result.Path.Last();
            

            // TODO: Check Capture
            // TODO: Check Finish
            // TODO: Check Win
            // TODO: Check Extra Turn
            
            // Publish signals
            // Send network messages
        }
    }
}