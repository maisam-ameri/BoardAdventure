using BoardAdventures.Core.Players;

namespace BoardAdventures.Core.GameLogic
{
    public interface ITurnFlowService
    {
        public  Player ActivePlayer { get; }
        public  Player PreviousPlayer { get; }
        void NextPlayer();
        bool IsActivePlayerTurn();
    }
}