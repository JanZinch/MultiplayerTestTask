using System;
using System.Collections.Generic;
using Common;
using Controllers;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class PlayerSessionManager : NetworkBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerCanvas _followCanvasOriginal;
        [SerializeField] private Transform _followCanvasPoint;

        private static LinkedList<PlayerController> _alivePlayers = new LinkedList<PlayerController>();


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

            _alivePlayers.AddLast(_playerController);
        }
        
    }
}