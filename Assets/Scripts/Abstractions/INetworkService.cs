using System.Collections.Generic;
using Photon.Realtime;

namespace BoardAdventures.Abstractions
{
    public interface INetworkService
    {
        public byte MaxPlayers { get; }
        public bool IsMasterClient { get; }
        void Connect();
        void JoinToRoom(byte maxPlayer);
        public void SetPlayerReady<T>(string key, T prop = default);
        public T GetPlayerProp<T>(string key,Player player = null, T defaultValue = default);
        List<Player>  GetPlayers();
        bool CheckAllPlayersReady();
        void LoadLevel(string levelName);
    }
}