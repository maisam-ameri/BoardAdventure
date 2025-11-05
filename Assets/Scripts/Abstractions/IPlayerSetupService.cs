using System.Collections.Generic;
using BoardAdventures.Core.Players;
using Core.Data;

namespace BoardAdventures.Abstractions
{
    public interface IPlayerSetupService
    {
        List<Player>  Players { get;}
        void Setup(GameMode mode);
    }
}