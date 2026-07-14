using System;
using BoardAdventures.Core.Commands;
using BoardAdventures.Core.Results;
using Core.State;

namespace BoardAdventures.Core.Rules
{
    public class MovePawnRule
    {
        public MovePawnResult Execute(MatchState state, MovePawnCommand command)
        {
            var step = state.DiceState.Step;
            var pawnId = command.PawnId;
            var isMoveValid = false;
            var isCapture = false;

            /*
             isMoveValid = validate path
             isCapture = check capture 
             */

            return new MovePawnResult{IsMoveValid = isMoveValid , IsCapture = isCapture};
        }
    }


}