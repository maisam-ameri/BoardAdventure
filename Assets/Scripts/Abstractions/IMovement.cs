using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodes.Abstractions;
using Pawns;


namespace Abstractions
{
    public interface IMovement
    {
        public event Action<IPawn> OnCaptured;
        Task Move(IPawn pawn, List<INode> path, Action onCompleted);
    }
}