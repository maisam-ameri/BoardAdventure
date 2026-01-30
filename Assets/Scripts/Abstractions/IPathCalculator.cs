using System.Collections.Generic;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Abstractions
{
    public interface IPathCalculator
    {
        public List<INode> DefinePath(int? step, IPawn pawn);
    }
}