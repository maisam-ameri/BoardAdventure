using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface IGameFlowService
    {
        public Player CurrentPlayer { get; }

        public void SwitchTurn();
        public void GrantReward(Player player);
        public void HandlePlayerActionCompleted();
        public void HandlePlayerActionStarted();
        public void StartGame();
    }
}