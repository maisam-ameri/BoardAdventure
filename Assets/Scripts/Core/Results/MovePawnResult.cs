using System.Collections.Generic;

namespace BoardAdventures.Core.Results
{
    public class MovePawnResult
    {
        public MoveFailReason FailReason { get; }
        public IReadOnlyList<int> Path { get; }

        public MovePawnResult(MoveFailReason failReason, IReadOnlyList<int> path = null)
        {
            FailReason = failReason;
            Path = path;
        }
    }
}