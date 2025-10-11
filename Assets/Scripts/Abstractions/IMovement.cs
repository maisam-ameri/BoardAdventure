using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Abstractions
{
    public interface IMovement
    {
        public event Action<IPawn> OnCaptured;
        Task Move(IPawn pawn, List<INode> path, Action onCompleted);
    }
}