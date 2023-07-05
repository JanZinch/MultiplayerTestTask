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

        public GameEndPopup Initialize(PlayerProfile winner)
        {
            _winnerNameMessage.text = WinnerNamePrefix + winner.Name;
            _winnerScoreMessage.text = WinnerScorePrefix + winner.Score;

            _exitButton.onClick.AddListener(Exit);
            
            Time.timeScale = 0.0f;
            
            return this;
        }
        
        public GameEndPopup Initialize(string winnerName, int winnerScore)
        {
            _winnerNameMessage.text = WinnerNamePrefix + winnerName;
            _winnerScoreMessage.text = WinnerScorePrefix + winnerScore;

            _exitButton.onClick.AddListener(Exit);
            
            Time.timeScale = 0.0f;
            
            return this;
        }

        private void Exit()
        {
            Time.timeScale = 1.0f;
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(Exit);
        }
    }
}