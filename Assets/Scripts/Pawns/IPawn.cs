using System;
using Factions;
using Nodes.Abstractions;
using UnityEngine;

namespace Pawns
{
    public interface IPawn
    {
        string State { get; set; }
        Color Color { get; set; }
        Vector2 Position { get; set; }
        Faction Faction { get; set; }
        INode CurrentNode { get; set; }
        public Collider2D Collider { get; }

        event Action<IPawn> OnSelectPawn;
    }
}