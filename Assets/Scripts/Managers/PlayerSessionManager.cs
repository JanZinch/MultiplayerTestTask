using System;
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

        private void Start()
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
        }
    }
}