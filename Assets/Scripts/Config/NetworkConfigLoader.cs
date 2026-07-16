using System;
using System.IO;
using BoardAdventures.Config;
using UnityEngine;

namespace BoardAdventures.Config
{
    public static class NetworkConfigLoader
    {
        private const string ConfigFileName = "network_config.json";
        private const string ConfigFolder = "Secrets";

        public static string GetAppId()
        {
            var path = Path.GetFullPath(
                Path.Combine(
                    Application.dataPath,
                    $"../{ConfigFolder}/{ConfigFileName}"));
            
            Debug.Log(path);
            
            if (!File.Exists(path))
            {
                Debug.LogError("the json file not found: ");
                return string.Empty;
            }

            try
            {
                var json = File.ReadAllText(path);
                NetworkSecret config = JsonUtility.FromJson<NetworkSecret>(json);

                if (config == null || string.IsNullOrWhiteSpace(config.appId))
                {
                    Debug.LogError("AppId is missing in config file.");
                    return string.Empty;
                }


                return config.appId;
            }
            catch (Exception e)
            {
                Debug.LogError("the json file not found: " + e.Message);
                return string.Empty;
            }
        }
    }
}