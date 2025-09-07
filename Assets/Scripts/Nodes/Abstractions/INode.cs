using Pawns;
using UnityEngine;

namespace Nodes.Abstractions
{
    public interface INode
    {
        int Index { get; set; }
        bool IsEmpty { get; set; }
        INode PrevNode { get; set; }
        INode NextNode { get; set; }
        Vector2 Position { get; set; }
        Collider2D Collider2D { get; }
        public IPawn Pawn { get; set; }
    }
}