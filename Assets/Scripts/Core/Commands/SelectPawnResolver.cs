using BoardAdventures.Core.Rules;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Commands
{
    public class SelectPawnResolver
    {
        public ICommand Resolve(MatchState state, SelectPawnCommand command, CommandContext context)
        {
            var selectPawnRule = new SelectPawnRule();
            var selectPawnCommand = new SelectPawnCommand(command.PawnId);
            var selectPawnResult = selectPawnRule.Execute(state, selectPawnCommand, context);

            if (selectPawnResult.CanMove)
                return new MovePawnCommand(command.PawnId);
            
            //if(selectPawnResult.CanPut)
                // return new PutPawnCommand(pawnId);

                return null;
        }
    }
}