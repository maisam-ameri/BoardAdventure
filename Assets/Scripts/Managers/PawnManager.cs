using System;
using Abstractions;
using Factions;
using Nodes.Abstractions;
using Pawns;

namespace Managers
{
    public class PawnManager
    {
        private readonly IPawnFactory _pawnFactory;
        private readonly IPawnStateService _pawnStateService;

        public PawnManager(IPawnFactory pawnFactory, IPawnStateService pawnStateService)
        {
            _pawnFactory = pawnFactory;
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