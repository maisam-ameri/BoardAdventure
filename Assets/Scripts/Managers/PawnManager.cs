using BoardAdventures.Board;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;
using BoardAdventures.Managers;
using BoardAdventures.Signals;
using Gameplay.GameLogic;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class PawnManager
    {
        private readonly IPawnFactory _pawnFactory;
        private readonly PawnStateService _pawnStateService;

        public PawnManager(IPawnFactory pawnFactory, PawnStateService pawnStateService, SignalBus signalBus)
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