using System;
using Factions;
using Nodes.Abstractions;
using UnityEngine;

namespace Pawns
{
    public class Pawn : MonoBehaviour, IPawn
    {
        public string State { get; set; } = "InBase";
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
                Color = value ? _tempColor.Value : Color = Color.gray;
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