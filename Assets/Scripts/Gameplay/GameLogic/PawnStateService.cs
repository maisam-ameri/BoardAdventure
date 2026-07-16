using System.Linq;
using BoardAdventures.Board;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;
using BoardAdventures.Signals;
using Zenject;

namespace Gameplay.GameLogic
{
    public class PawnStateService
    {

        private readonly SignalBus _signalBus;

        public PawnStateService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void EnterPawnToGame(IPawn pawn)
        {
            _signalBus.Fire(new OnPlayerActionStartedSignal());
            
            pawn.Position = pawn.Faction.StartNode.Position;
            pawn.CurrentNode.IsEmpty = true;
            pawn.CurrentNode.Pawn = null;
            pawn.CurrentNode = pawn.Faction.StartNode;
            pawn.Faction.StartNode.IsEmpty = false;
            pawn.LocationState = PawnLocationState.InGame;
            
            _signalBus.Fire(new OnPlayerActionCompletedSignal());
        }

        public void ReturnPawnToBase(IPawn pawn)
        {
            var emptyBaseNode = GetEmptyBaseNode(pawn.Faction);
            
            if (emptyBaseNode is null)
                return;
            
            pawn.Position = emptyBaseNode.Position;
            pawn.CurrentNode.Pawn = null;
            pawn.CurrentNode.IsEmpty = true;
            pawn.CurrentNode = emptyBaseNode;
            emptyBaseNode.Pawn = pawn;
            emptyBaseNode.IsEmpty = false;
            pawn.LocationState = PawnLocationState.InBase;
        }
        
        private static INode GetEmptyBaseNode(Faction faction) =>
            faction.BaseNodes.FirstOrDefault(n => n.IsEmpty);

    }
}