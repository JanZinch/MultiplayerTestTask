using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameEndPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _winnerNameMessage;
        [SerializeField] private TextMeshProUGUI _winnerScoreMessage;
        [SerializeField] private Button _exitButton;
        
        private const string WinnerNamePrefix = "Winner: ";
        private const string WinnerScorePrefix = "His score: ";

        public Action _onExitCallback = null;
        
        public GameEndPopup Initialize(string winnerName, int winnerScore, Action onExitCallback)
        {
            _winnerNameMessage.text = WinnerNamePrefix + winnerName;
            _winnerScoreMessage.text = WinnerScorePrefix + winnerScore;

            _onExitCallback = onExitCallback;
            _exitButton.onClick.AddListener(Exit);
            
            Time.timeScale = 0.0f;
            
            return this;
        }

        private void Exit()
        {
            Time.timeScale = 1.0f;
            _onExitCallback?.Invoke();
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(Exit);
        }
    }
}