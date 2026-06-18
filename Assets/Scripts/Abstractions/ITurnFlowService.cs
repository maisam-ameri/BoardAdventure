using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface ITurnFlowService
    {
        public  Player ActivePlayer { get; }
        public  Player PreviousPlayer { get; }
        void NextPlayer();
        bool IsActivePlayerTurn();
    }
}