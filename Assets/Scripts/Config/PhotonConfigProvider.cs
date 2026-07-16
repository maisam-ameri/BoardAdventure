using BoardAdventures.Config;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace BoardAdventures.Config
{
    public class PhotonConfigProvider : INetworkConfigProvider
    {
        public AppSettings GetConfig()
        {
            
            var appId = NetworkConfigLoader.GetAppId();
            var settings = PhotonNetwork.PhotonServerSettings.AppSettings;

            if (string.IsNullOrEmpty(appId))
            {
                Debug.LogError("Photon initialization aborted.");
                return settings;
            }

            

            var runtimeSettings = new AppSettings()
            {
                AppIdRealtime = appId,
                FixedRegion = "asia",
                Server = settings.Server,
                Port = settings.Port,
                Protocol = settings.Protocol
            };

            return runtimeSettings;

            // PhotonNetwork.ConnectUsingSettings(runtimeSettings);
        }
    }


}