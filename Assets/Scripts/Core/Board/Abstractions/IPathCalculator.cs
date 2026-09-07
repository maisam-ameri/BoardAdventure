using System.Collections.Generic;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Board
{
    public interface IPathCalculator
    {
        IReadOnlyList<byte> Calculate(PawnState pawn, byte? step);
    }
}