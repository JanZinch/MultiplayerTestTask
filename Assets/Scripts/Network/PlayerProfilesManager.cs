using System;
using System.Collections.Generic;
using Configs;
using Controllers;
using Managers;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Network
{
    public class PlayerProfilesManager : NetworkBehaviour
    {
        [SerializeField] private PlayerColorsConfig _playerColorsConfig;
        
        public static PlayerProfilesManager Instance { get; private set; }
        public string LocalPlayerName { get; private set; }
        private LinkedList<PlayerProfile> _alivePlayers = null;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void InitLocalPlayer(string localPlayerName, Role playerRole)
        {
            LocalPlayerName = localPlayerName;

            if (playerRole == Role.Host)
            {
                _alivePlayers = new LinkedList<PlayerProfile>();
            }
        }

        public Color GetRandomPlayerColor()
        {
            return _playerColorsConfig.GetRandomColor();
        }
        
        public void OnPlayerSpawn(PlayerProfile player)
        {
            _alivePlayers.AddLast(player);
        }
        
        public void OnPlayerDespawn(PlayerProfile player)
        {
            _alivePlayers.Remove(player);

            if (_alivePlayers.Count == 1)
            {
                PlayerProfile winner = _alivePlayers.First.Value;
                ShowGameEndPopupClientRpc(winner.Name, winner.Score);
            }
        }

        [ClientRpc]
        private void ShowGameEndPopupClientRpc(string winnerName, int winnerScore)
        {
            HeadUpDisplay.Instance.ShowGameEndPopup(winnerName, winnerScore, Cleanup);
        }

        private void Cleanup()
        {
            NetworkManager.Singleton.Shutdown();

            if (_alivePlayers != null)
            {
                _alivePlayers.Clear();
            }

            LocalPlayerName = string.Empty;

            if (NetworkManager.Singleton != null)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }

            SceneManager.LoadScene("Lobby");
        }
    }
}