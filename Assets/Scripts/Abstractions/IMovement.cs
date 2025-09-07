using System.Collections.Generic;
using System.Threading.Tasks;
using Nodes.Abstractions;
using Pawns;


namespace Abstractions
{
    public interface IMovement
    {
        Task Move(IPawn pawn, List<INode> path);
    }
}