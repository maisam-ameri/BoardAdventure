using System.Collections.Generic;
using BoardAdventures.Core.Players;
using ExitGames.Client.Photon;

namespace BoardAdventures.Abstractions
{
    public interface INetworkService
    {
        public byte MaxPlayers { get; }
        public bool IsMasterClient { get; }
        void Connect();
        void JoinToRoom(byte maxPlayer);
        void SetPlayerReady(bool isReady);
        bool CheckAllPlayersReady();
        void LoadLevel(string levelName);
        Hashtable GetPlayerCustomProperties();
        List<Player>  GetPlayers();
    }
}