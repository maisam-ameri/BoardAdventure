using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using UnityEngine;

namespace BoardAdventures.GameObjects.Pawns.Abstractions
{
    public interface IPawn
    {
        PawnState State { get; set; }
        Color Color { get; set; }
        Vector2 Position { get; set; }
        Faction Faction { get; set; }
        INode CurrentNode { get; set; }
        Collider2D Collider { get; }
        bool IsActive { set; }

    }
}