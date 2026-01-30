using System;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Abstractions
{
    public interface IPawnStateService
    {
        public void EnterPawnToGame(IPawn pawn);

        public void ReturnPawnToBase(IPawn pawn);
    }
}