using System;
using System.Collections.Generic;
using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface ITurnFlowService
    {
        public  Player CurrentPlayer { get; }
        public  Player LastPlayer { get; }
        event Action OnTurnSwitched;
        event Action<Player, Player> OnTurnStarted;
        //void Initialize(List<Player> players);
        void StartTurn();
        void SwitchTurn();
    }
}