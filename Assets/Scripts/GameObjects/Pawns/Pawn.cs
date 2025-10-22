using System;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using UnityEngine;

namespace BoardAdventures.GameObjects.Pawns
{
    public class Pawn : MonoBehaviour, IPawn
    {
        public PawnState State { get; set; } = PawnState.InBase;
        public Color Color
        {
            get => GetComponent<SpriteRenderer>().color;
            set
            {
                _tempColor ??= value;
                GetComponent<SpriteRenderer>().color = value;
            }
        }

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public bool IsActive
        {
            set
            {
                Color = value ? _tempColor ?? Color : Color = Color.gray;
                Collider.enabled = value;
            } 
        }
        public Faction Faction { get; set; }
        public INode CurrentNode { get; set; }
        public Collider2D Collider => GetComponent<Collider2D>();

        public event Action<IPawn> OnSelectPawn;
        private Color? _tempColor;
        
        
        private void OnMouseDown()
        {
            OnSelectPawn?.Invoke(this);
        }

    }
}