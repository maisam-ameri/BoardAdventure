using System.Collections.Generic;
using Gameplay.Players;

namespace Gameplay.GameLogic.Abstractions
{
    public interface IPlayerSetupService
    {
        List<Player>  Players { get;}
        void Setup(List<Player> players);
    }
}