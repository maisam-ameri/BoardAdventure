using System;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Abstractions
{
    public interface IPawnStateService
    {
        public void EnterPawnToGame(IPawn pawn,Action onActionStarted, Action onActionCompleted);

        public void ReturnPawnToBase(IPawn pawn);
    }
}