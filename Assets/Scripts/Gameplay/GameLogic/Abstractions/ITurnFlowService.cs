using Gameplay.Players;

namespace Gameplay.GameLogic.Abstractions
{
    public interface ITurnFlowService
    {
        public  Player ActivePlayer { get; }
        public  Player PreviousPlayer { get; }
        void NextPlayer();
        bool IsActivePlayerTurn();
    }
}