using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using UnityEngine;

namespace BoardAdventures.GameObjects.Nodes
{
    public abstract class Node : MonoBehaviour, INode
    {
        public int Index { get; set; }
        public bool IsEmpty { get; set; } = true;
        public INode PrevNode { get; set; }
        public INode NextNode { get; set; }

        public Collider2D Collider2D => GetComponent<Collider2D>();
        
        public IPawn Pawn { get; set; }
    
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }


    }
}