﻿using System;
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
        
        private NetworkVariable<FixedString64Bytes> _name = new NetworkVariable<FixedString64Bytes>(default, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        
        private NetworkVariable<Color> _color = new NetworkVariable<Color>(default, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        
        public string Name => _name.Value.ToString();
        public int Score => _playerController.Score;
        
        public override void OnNetworkSpawn()
        {
            PlayerCanvas playerCanvas = Instantiate<PlayerCanvas>(_followCanvasOriginal, _followCanvasPoint.position, Quaternion.identity);
            playerCanvas.SetTargetToFollow(_playerController.transform);
            _playerController.InjectViews(playerCanvas.HealthBar, playerCanvas.CoinsCounter);
            
            _name.OnValueChanged += (old, actual) =>
            {
                //Debug.Log("On value changed");
                playerCanvas.SetPlayerName(actual.ToString());
            };
            
            _color.OnValueChanged += (old, actual) =>
            {
                _playerController.SetColor(actual);
            };
            
            if (IsOwner)
            {
                _playerController.InjectControlDevices(HeadUpDisplay.Instance.MotionJoystick, HeadUpDisplay.Instance.ShootingButton);
                _name.Value = PlayerProfilesManager.Instance.LocalPlayerName;
                _color.Value = PlayerProfilesManager.Instance.GetRandomPlayerColor();
            }
            
            playerCanvas.SetPlayerName(_name.Value.ToString());
            _playerController.SetColor(_color.Value);
            
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