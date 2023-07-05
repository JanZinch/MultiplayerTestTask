using System;
using System.Collections.Generic;
using Configs;
using Controllers;
using Managers;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network
{
    public class PlayerProfilesManager : MonoBehaviour
    {
        [SerializeField] private PlayerColorsConfig _playerColorsConfig;
        [SerializeField] private GameEndPopup _gameEndPopupOriginal;
        
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

        //[ServerRpc]
        public void OnPlayerSpawn(PlayerProfile player)
        {
            _alivePlayers.AddLast(player);
        }
        
        //[ServerRpc]
        public void OnPlayerDespawn(PlayerProfile player)
        {
            _alivePlayers.Remove(player);

            if (_alivePlayers.Count == 1)
            {
                OnGameEnd();
            }
        }
        
        private void OnGameEnd()
        {
            Instantiate<GameEndPopup>(_gameEndPopupOriginal).Initialize(_alivePlayers.First.Value);
        }
    }
}