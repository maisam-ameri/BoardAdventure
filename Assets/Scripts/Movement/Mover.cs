using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Nodes.Abstractions;
using Path;
using Pawns;

namespace Movement
{
    public class Mover : IMovement
    {

        private readonly int _delay;
        public event Action<IPawn> OnCaptured;
        
        
        public Mover(int delay = 500)
        {
            _delay = delay;
        }


        public async Task Move(IPawn pawn, List<INode> path, Action onCompleted = null)
        {
            if (path == null || path.Count == 0) return;

            pawn.CurrentNode.Pawn = null;
            pawn.CurrentNode.IsEmpty = true;

            foreach (var node in path)
            {

                if (path.Count > 1 && node.Equals(path[^2]))
                {
                    if (PathValidator.CheckNodeToCapture(pawn, path[^1]))
                    {
                        OnCaptured?.Invoke(path[^1].Pawn);
                        //_pawnStateService.ReturnPawnToBase(path[^1].Pawn,null);
                    }
                }
                pawn.Position = node.Position;
                await Task.Delay(_delay);
            }

            pawn.CurrentNode = path[^1];
            pawn.CurrentNode.Pawn = pawn;
            pawn.CurrentNode.IsEmpty = false;
            onCompleted?.Invoke();
        }

    }
}