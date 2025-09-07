using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Nodes.Abstractions;
using Pawns;
using Unity.VisualScripting;

namespace Movement
{
    public class Mover : IMovement
    {

        private readonly int _delay;
        private event Action OnMoveCompleted;
        
        
        public Mover(int delay = 1000,Action onMoveCompleted = null)
        {
            OnMoveCompleted = onMoveCompleted;
            _delay = delay;
        }
        public async Task Move(IPawn pawn, List<INode> path)
        {
            var maxStep = path.Count ;
            var currentStep = 0;

            path[currentStep].PrevNode.IsEmpty = true;
            path[currentStep].PrevNode.Pawn = null;
            
            while (currentStep < maxStep)
            {
                pawn.Position = path[currentStep].Position;
                currentStep++;

                await Task.Delay(_delay);
            }

            pawn.CurrentNode = path[^1];
            pawn.CurrentNode.Pawn = pawn;
            path[^1].IsEmpty = false;
         
            OnMoveCompleted?.Invoke();


        }
    }
}