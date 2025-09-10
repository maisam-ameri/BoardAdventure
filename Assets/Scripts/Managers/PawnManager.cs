using Factions;
using Nodes.Abstractions;
using Pawns;
using UnityEngine;

namespace Managers
{
    public class PawnManager : MonoBehaviour
    {
        [SerializeField] private Pawn pawnPrefab;
        private PawnFactory _pawnFactory;

        private void Start()
        {
            _pawnFactory = new PawnFactory(pawnPrefab);
        }
        
        public IPawn CreatePawn(Faction faction, INode baseNode)
        {
            var newPawn =  _pawnFactory.Create();

            newPawn.Color = faction.Color;
            newPawn.Position = baseNode.Position;
            newPawn.Faction = faction;
            //newPawn.Collider.enabled = false;
            newPawn.CurrentNode = baseNode;
            faction.Pawns.Add(newPawn);
            baseNode.IsEmpty = false;

            return newPawn;
        }
    }
}