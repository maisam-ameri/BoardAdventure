using System.Collections.Generic;
using Photon.Realtime;

namespace BoardAdventures.Network
{
    public interface INetworkService
    {
        byte MaxPlayers { get; }
        bool IsMasterClient { get; }
        Player LocalPlayer { get; }
        void Connect();
        void JoinToRoom(byte maxPlayer);
        void SetCustomProperty<T>(string key, T prop = default);
        void SetPlayerCustomProperty<T>(string key, T prop = default);
        T GetRoomProp<T>(string key, T defaultValue = default);
        T GetPlayerProp<T>(string key, Player player = null, T defaultValue = default);
        List<Player> GetPlayers();
        bool CheckAllPlayersReady();
        void LoadLevel(string levelName);
    }
}