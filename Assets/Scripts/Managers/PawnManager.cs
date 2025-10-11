using System;
using Abstractions;
using Factions;
using GameLogic;
using Nodes.Abstractions;
using Pawns;
using UnityEngine;

namespace Managers
{
    public class PawnManager : MonoBehaviour
    {
        [SerializeField] private Pawn pawnPrefab;
        private IPawnFactory _pawnFactory;
        private IPawnStateService _pawnStateService;

       

        private void Start()
        {
            _pawnFactory = new PawnFactory(pawnPrefab);
            _pawnStateService = new PawnStateService();
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

        public void EnterPawnToGame(IPawn pawn, Action onPawnEntered)
        {
            _pawnStateService.EnterPawnToGame(pawn, onPawnEntered);
        }

        public void ReturnPawnToBase(IPawn pawn)
        {
            _pawnStateService.ReturnPawnToBase(pawn);
        }
    }
}