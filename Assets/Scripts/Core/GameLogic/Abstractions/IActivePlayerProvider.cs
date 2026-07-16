using BoardAdventures.Core.Players;

namespace BoardAdventures.Core.GameLogic
{
    public interface IActivePlayerProvider
    {
        public Player ActivePlayer { get; }
    }
}