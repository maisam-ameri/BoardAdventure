using System;
using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class PawnManager : MonoBehaviour
    {
        [SerializeField] private Pawn pawnPrefab;
        private IPawnFactory _pawnFactory;
        private IPawnStateService _pawnStateService;


        public void Initialize(IPawnFactory pawnFactory, IPawnStateService pawnStateService)
        {
            _pawnFactory = pawnFactory;
            _pawnFactory = new PawnFactory(pawnPrefab);
            _pawnStateService = pawnStateService;
        }

        public IPawn CreatePawn(Faction faction, INode baseNode)
        {
            var newPawn =  _pawnFactory.Create();

            newPawn.Color = faction.Color;
            newPawn.Position = baseNode.Position;
            newPawn.Faction = faction;
            newPawn.CurrentNode = baseNode;
            faction.Pawns.Add(newPawn);
            baseNode.IsEmpty = false;

            return newPawn;
        }

        public void EnterPawnToGame(IPawn pawn,Action onActionStarted, Action onActionCompleted)
        {
            _pawnStateService.EnterPawnToGame(pawn,onActionStarted, onActionCompleted);
        }

        public void ReturnPawnToBase(IPawn pawn)
        {
            _pawnStateService.ReturnPawnToBase(pawn);
        }
    }
}