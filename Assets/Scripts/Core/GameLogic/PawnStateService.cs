using System;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Core.GameLogic
{
    public class PawnStateService: IPawnStateService
    {
        public void EnterPawnToGame(IPawn pawn,Action onActionStarted, Action onActionCompleted)
        {
            onActionStarted?.Invoke();
            pawn.Position = pawn.Faction.StartNode.Position;
            pawn.CurrentNode.IsEmpty = true;
            pawn.CurrentNode.Pawn = null;
            pawn.CurrentNode = pawn.Faction.StartNode;
            pawn.Faction.StartNode.IsEmpty = false;
            pawn.State = PawnState.InGame;
            onActionCompleted?.Invoke();
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