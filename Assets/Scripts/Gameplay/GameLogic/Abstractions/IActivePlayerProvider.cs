using BoardAdventures.Gameplay.Players;

namespace BoardAdventures.Gameplay.GameLogic.Abstractions
{
    public interface IActivePlayerProvider
    {
        public Player ActivePlayer { get; }
    }
}