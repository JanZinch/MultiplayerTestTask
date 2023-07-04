using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HeadUpDisplay : MonoBehaviour
    {
        [SerializeField] private Joystick _motionJoystick;
        [SerializeField] private Button _shootingButton;

        [SerializeField] private Button _btnHost;
        [SerializeField] private Button _btnServer;
        [SerializeField] private Button _btnClient;
        
        public Joystick MotionJoystick => _motionJoystick;
        public Button ShootingButton => _shootingButton;
        
        public static HeadUpDisplay Instance { get; set; } = null;

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

        private void OnEnable()
        {
            _btnHost.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
            });
            _btnServer.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartServer();
            });
            _btnClient.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
            });
        }
    }
}