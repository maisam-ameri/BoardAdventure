using System.Collections.Generic;
using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface IPlayerSetupService
    {
        List<Player>  Players { get;}
        void Setup(List<Player> players);
    }
}