using System;
using System.Collections.Generic;
using Common;
using Controllers;
using Network;
using UI;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class PlayerProfile : NetworkBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerCanvas _followCanvasOriginal;
        [SerializeField] private Transform _followCanvasPoint;
        
        private NetworkVariable<FixedString64Bytes> _playerName = new NetworkVariable<FixedString64Bytes>(default, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public string Name => _playerName.Value.ToString();
        public int Score => _playerController.Score;
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                _playerController.InjectControlDevices(HeadUpDisplay.Instance.MotionJoystick, HeadUpDisplay.Instance.ShootingButton);
                _playerController.SetColor(Color.yellow);
            }
            else
            {
                _playerController.SetColor(Color.green);
            }

            PlayerCanvas playerCanvas = Instantiate<PlayerCanvas>(_followCanvasOriginal, _followCanvasPoint.position, Quaternion.identity);
            playerCanvas.SetTargetToFollow(_playerController.transform);
            _playerController.InjectViews(playerCanvas.HealthBar, playerCanvas.CoinsCounter);

            _playerName.OnValueChanged += (old, actual) =>
            {
                Debug.Log("On value changed");
                
                playerCanvas.SetPlayerName(actual.ToString());
            };
            
            if (IsOwner)
            {
                Debug.Log("Try get name");
                
                _playerName.Value = PlayerProfilesManager.Instance.LocalPlayerName;
            }
            
            playerCanvas.SetPlayerName(_playerName.Value.ToString());
            
            if (IsHost)
            {
                PlayerProfilesManager.Instance.OnPlayerSpawn(this);
                _playerController.OnDeath += () =>
                {
                    PlayerProfilesManager.Instance.OnPlayerDespawn(this);
                };

            }
        }
    }
}