using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HeadUpDisplay : MonoBehaviour
    {
        [SerializeField] private Joystick _motionJoystick;
        [SerializeField] private Button _shootingButton;
        
        [Space]
        [SerializeField] private GameEndPopup _gameEndPopupOriginal;
        
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
        
        public void ShowGameEndPopup(string winnerName, int winnerScore, Action onExitCallback)
        {
            Instantiate<GameEndPopup>(_gameEndPopupOriginal, transform).Initialize(winnerName, winnerScore, onExitCallback);
        }
    }
}