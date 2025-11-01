using System;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Abstractions
{
    public interface IPawnManager
    {
        //void Initialize(IPawnFactory pawnFactory, IPawnStateService pawnStateService);
        IPawn CreatePawn(Faction faction, INode baseNode);
        void EnterPawnToGame(IPawn pawn, Action onActionStarted, Action onActionCompleted);
        void ReturnPawnToBase(IPawn pawn);
    }
}