using UnityEngine;

namespace Nodes
{
    public class BaseNode: Node
    {
        public Color Color
        {
            get => GetComponent<SpriteRenderer>().color;
            set => GetComponent<SpriteRenderer>().color = value;
        }
    }
}