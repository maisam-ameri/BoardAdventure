using System;
using System.Collections.Generic;
using System.Linq;
using Nodes;
using Pawns;
using UnityEngine;

namespace Factions
{
    public class Faction : MonoBehaviour
    {
        public string Name;
        public Color Color = Color.red;
        public Node StartNode;
        public Node GatewayNode;
        public List<Node> GoalNodes;
        public List<Node> BaseNodes;
        public bool IsCompletedGoals { get; private set; }
        public List<IPawn> Pawns { get; set; }

        public event Action<Faction> OnSelectFaction;
        public event Action OnCompletedGoals;

        private void OnMouseDown()
        {
            if(OnSelectFaction == null) Debug.Log("it is null");
            OnSelectFaction?.Invoke(this);
        }

        public void CheckGoalCompletion()
        {
            if (!GoalNodes.Any(n => n.IsEmpty)) return;
            
            IsCompletedGoals = true;
            OnCompletedGoals?.Invoke();
        }
    }
}