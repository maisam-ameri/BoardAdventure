using Gameplay.Players;

namespace Gameplay.GameLogic.Abstractions
{
    public interface IActivePlayerProvider
    {
        public Player ActivePlayer { get; }
    }
}