using System;
using Controllers;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class PlayerSessionManager : NetworkBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        
        private void Start()
        {
            if (IsOwner)
            {
                _playerController.InjectControllers(HeadUpDisplay.Instance.MotionJoystick, HeadUpDisplay.Instance.ShootingButton);
            }
        }
    }
}