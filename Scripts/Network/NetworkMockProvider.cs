using System.Collections.Generic;
using BoardAdventures.Abstractions;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardAdventures.Network
{
    public class NetworkMockProvider: MonoBehaviour , INetworkService
    {
        public void Connect()
        {
            Debug.Log("connected(Mock)");
        }

        public void JoinToRoom(byte maxPlayer)
        {
            Debug.Log("JoinToRoom(Mock)");
            SceneManager.LoadScene("Match");
        }

        public void SetPlayerReady(bool isReady)
        {
            Debug.Log("SetPlayerReady(Mock)");
        }

        public List<Core.Players.Player> GetPlayers()
        {
            var players = new List<Core.Players.Player>
            {
                new()
                {
                    Nickname = "mesi",
                },
                new ()
                {
                    Nickname = "karen"
                }
            };
            
            return players;
        }
    }
}