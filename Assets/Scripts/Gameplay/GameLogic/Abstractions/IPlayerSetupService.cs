using System.Collections.Generic;
using BoardAdventures.Gameplay.Players;

namespace BoardAdventures.Gameplay.GameLogic.Abstractions
{
    public interface IPlayerSetupService
    {
        List<Player>  Players { get;}
        void Setup(List<Player> players);
    }
}