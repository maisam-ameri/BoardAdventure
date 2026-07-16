using UnityEngine;

namespace BoardAdventures.Board.Nodes
{
    public class BaseNode: Node
    {
        public Color Color
        {
            set => GetComponent<SpriteRenderer>().color = value;
        }
    }
}