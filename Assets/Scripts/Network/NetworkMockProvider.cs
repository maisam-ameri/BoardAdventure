using System.Collections.Generic;
using BoardAdventures.Abstractions;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player = BoardAdventures.Core.Players.Player;

namespace BoardAdventures.Network
{
    public class NetworkMockProvider : MonoBehaviour, INetworkService
    {
        public byte MaxPlayers => 2;
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;

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

        public bool CheckAllPlayersReady()
        {
            return false;
        }

        public void LoadLevel(string levelName)
        {
            PhotonNetwork.LoadLevel(levelName);
        }

        public Hashtable GetPlayerCustomProperties()
        {
            return null;
        }

        public List<Player> GetPlayers()
        {
            var players = new List<Player>
            {
                new()
                {
                    Nickname = "mesi",
                },
                new()
                {
                    Nickname = "karen"
                }
            };

            return players;
        }
    }
}