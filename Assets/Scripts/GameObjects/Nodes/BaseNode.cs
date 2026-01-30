using UnityEngine;

namespace BoardAdventures.GameObjects.Nodes
{
    public class BaseNode: Node
    {
        public Color Color
        {
            set => GetComponent<SpriteRenderer>().color = value;
        }
    }
}