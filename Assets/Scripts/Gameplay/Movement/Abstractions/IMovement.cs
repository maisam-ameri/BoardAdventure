using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;

namespace Gameplay.Movement.Abstractions
{
    public interface IMovement
    {
        Task Move(IPawn pawn, List<INode> path);
    }
}