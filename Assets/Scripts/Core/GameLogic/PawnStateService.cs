using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class PawnStateService: IPawnStateService
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
            pawn.State = PawnState.InGame;
            
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
            pawn.State = PawnState.InBase;
        }
        
        private static INode GetEmptyBaseNode(Faction faction) =>
            faction.BaseNodes.FirstOrDefault(n => n.IsEmpty);

    }
}