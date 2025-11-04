using System;
using Core.Data;

namespace BoardAdventures.Abstractions
{
    public interface IGameManager
    {
        public void StartGame(GameMode mode);
    }
}