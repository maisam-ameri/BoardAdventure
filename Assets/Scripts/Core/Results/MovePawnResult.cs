using System.Collections.Generic;

namespace BoardAdventures.Core.Results
{
    public class MovePawnResult
    {
        public byte PawnId { get;  }
        public MoveFailReason FailReason { get; }
        public IReadOnlyList<int> Path { get; }

        public MovePawnResult(byte pawnId, MoveFailReason failReason, IReadOnlyList<int> path = null)
        {
            PawnId = pawnId;
            Path = path;
            FailReason = failReason;
        }
    }
}