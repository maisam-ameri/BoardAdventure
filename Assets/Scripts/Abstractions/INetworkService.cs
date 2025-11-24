using System.Collections.Generic;
using Photon.Realtime;

namespace BoardAdventures.Abstractions
{
    public interface INetworkService
    {
        void Connect();
        void JoinToRoom(byte maxPlayer);
        void SetPlayerReady(bool isReady);
        List<Player>  GetPlayers();
    }
}