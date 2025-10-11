using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Path;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Core.Movement
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