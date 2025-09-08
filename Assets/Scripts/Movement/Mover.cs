using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Nodes.Abstractions;
using Pawns;

namespace Movement
{
    public class Mover : IMovement
    {

        private readonly int _delay;
        
        
        public Mover(int delay = 1000)
        {
            _delay = delay;
        }

        public async Task Move(IPawn pawn, List<INode> path)
        {
            if (path == null || path.Count == 0) return;

            pawn.CurrentNode.Pawn = null;
            pawn.CurrentNode.IsEmpty = true;

            foreach (var node in path)
            {
                pawn.Position = node.Position;
                await Task.Delay(_delay);
            }

            pawn.CurrentNode = path[^1];
            pawn.CurrentNode.Pawn = pawn;
            pawn.CurrentNode.IsEmpty = false;
        }
    }
}