using System.Collections.Generic;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;

namespace BoardAdventures.Core.Path
{
    public interface IPathCalculator
    {
        public List<INode> DefinePath(int? step, IPawn pawn);
    }
}