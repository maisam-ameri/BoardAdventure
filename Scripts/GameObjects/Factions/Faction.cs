using System;
using System.Collections.Generic;
using System.Linq;
using BoardAdventures.GameObjects.Nodes;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using UnityEngine;

namespace BoardAdventures.GameObjects.Factions
{
    public class Faction : MonoBehaviour
    {
        public string Name;
        public Color Color = Color.red;
        private Color _tempColor;
        public Node StartNode;
        public Node GatewayNode;
        public List<Node> GoalNodes;
        public List<Node> BaseNodes;
        public List<IPawn> Pawns { get; set; }

        public event Action<Faction> OnSelectFaction;

        private void OnMouseDown()
        {
            if(OnSelectFaction == null) Debug.Log("it is null");
            OnSelectFaction?.Invoke(this);
        }

        public void SetActivate(bool isActive)
        {
            foreach (var node in BaseNodes.Cast<BaseNode>())
            {
                if (isActive)
                {
                    node.Color = _tempColor;
                }
                else
                {
                    _tempColor = Color;
                    Color = Color.gray;
                }
            }
        }
    }
}