using System;

namespace BoardAdventures.Abstractions
{
    public interface IGameManager
    {
        public Action OnStartGame { get; set; }
        public Action OnEndGame { get; set; }
    }
}