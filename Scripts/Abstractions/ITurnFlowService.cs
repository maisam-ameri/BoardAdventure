using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface ITurnFlowService
    {
        public  Player CurrentPlayer { get; }
        public  Player LastPlayer { get; }
        void NextPlayer();
    }
}