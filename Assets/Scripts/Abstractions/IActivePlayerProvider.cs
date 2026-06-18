using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface IActivePlayerProvider
    {
        public Player ActivePlayer { get; }
    }
}