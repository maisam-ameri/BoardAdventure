using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace BoardAdventures.Network
{
    public static class NetworkHelper
    {
        public static void SetPlayerCustomProperty(string key, object value)
        {
            if (PhotonNetwork.CurrentRoom == null) return;
            Hashtable prop = new Hashtable {{key, value}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
        }

        public static void SetCustomProperty(string key, object value)
        {
            if (PhotonNetwork.CurrentRoom == null) return;
            Hashtable prop = new Hashtable {{key, value}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }
        
        public static T GetPlayerCustomProperty<T>(string key,Player player)
        {
            if (PhotonNetwork.CurrentRoom == null) return default;
            
            if (player.CustomProperties.TryGetValue(key, out var value))
            {
                if (value is T typedValue)
                    return typedValue;
            }

            return default;
        }
        
        public static T GetCustomProperty<T>(string key)
        {
            if (PhotonNetwork.CurrentRoom == null) return default;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out var value))
            {
                if (value is T typedValue)
                    return typedValue;
            }

            return default;
        }
        
        public static TEnum GetEnumCustomProperty<TEnum>(string key) where TEnum : struct, Enum
        {
            if (PhotonNetwork.CurrentRoom == null) return default;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out var value))
            {
                if (value is int intValue)
                    return (TEnum) Enum.ToObject(typeof(TEnum), intValue);

                if (value is byte byteValue)
                    return (TEnum) Enum.ToObject(typeof(TEnum), byteValue);
            }

            return default;
        }
    }
}