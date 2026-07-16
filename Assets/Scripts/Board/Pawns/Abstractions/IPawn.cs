using BoardAdventures.Board.Nodes;
using UnityEngine;

namespace BoardAdventures.Board.Pawns
{
    public interface IPawn
    {
        PawnLocationState LocationState { get; set; }
        Color Color { get; set; }
        Vector2 Position { get; set; }
        Faction Faction { get; set; }
        INode CurrentNode { get; set; }
        Collider2D Collider { get; }
        bool IsActive { set; }

    }
}