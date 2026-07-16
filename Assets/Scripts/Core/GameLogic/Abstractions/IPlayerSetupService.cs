using System.Collections.Generic;
using BoardAdventures.Core.Players;

namespace BoardAdventures.Core.GameLogic
{
    public interface IPlayerSetupService
    {
        List<Player>  Players { get;}
        void Setup(List<Player> players);
    }
}