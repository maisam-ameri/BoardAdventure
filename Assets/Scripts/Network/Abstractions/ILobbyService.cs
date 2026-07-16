using System.Collections.Generic;
using Photon.Realtime;

namespace BoardAdventures.Network
{
    public interface ILobbyService
    {
        bool IsPlayerReady(Player player);
        void ToggleReady();
        void StartMatch(string levelName);
        List<Player> Players { get; }
    }
}