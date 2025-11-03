using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class PawnManager : IPawnManager
    {
        private readonly IPawnFactory _pawnFactory;
        private readonly IPawnStateService _pawnStateService;

        public PawnManager(IPawnFactory pawnFactory, IPawnStateService pawnStateService, SignalBus signalBus)
        {
            _pawnFactory = pawnFactory;
            _pawnStateService = pawnStateService;

            signalBus.Subscribe<OnCapturedSignal>(HandleCapturePawn);
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
        
        private void HandleCapturePawn(OnCapturedSignal signal)
        {
            ReturnPawnToBase(signal.Pawn);
        }

        private void ReturnPawnToBase(IPawn pawn)
        {
            _pawnStateService.ReturnPawnToBase(pawn);
        }
    }
}