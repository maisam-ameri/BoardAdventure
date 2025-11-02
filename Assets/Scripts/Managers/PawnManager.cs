using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Core.GameLogic
{
    public class PawnManager : IPawnManager
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

        public void EnterPawnToGame(IPawn pawn)
        {
            _pawnStateService.EnterPawnToGame(pawn);
        }

        public void ReturnPawnToBase(IPawn pawn)
        {
            _pawnStateService.ReturnPawnToBase(pawn);
        }
    }
}