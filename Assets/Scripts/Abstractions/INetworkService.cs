using System.Collections.Generic;
using BoardAdventures.Core.Players;

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