using System.Collections.Generic;
using Photon.Realtime;

namespace BoardAdventures.Abstractions
{
    public interface INetworkService
    {
        void Connect();
        void JoinToRoom();
        Dictionary<int, Player> GetPlayers();
    }
}