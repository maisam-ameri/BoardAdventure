using System;

namespace BoardAdventures.Abstractions
{
    public interface IGameInputHandler
    {
        event Action OnDiceRollRequested;
        void HandleDiceRollRequest();
    }
}