using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface ICurrentPlayerProvider
    {
        public Player CurrentPlayer { get; }
    }
}