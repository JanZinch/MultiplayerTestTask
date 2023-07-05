using System;
using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameEndPopup : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _winnerNameMessage;
        [SerializeField] private TextMeshProUGUI _winnerScoreMessage;
        [SerializeField] private Button _exitButton;
        [SerializeField] private NetworkObject _networkObject;
        
        private const string WinnerNamePrefix = "Winner: ";
        private const string WinnerScorePrefix = "His score: ";

        public GameEndPopup Initialize(PlayerProfile winner)
        {
            _networkObject.Spawn(true);
            
            _winnerNameMessage.text = WinnerNamePrefix + winner.Name;
            _winnerScoreMessage.text = WinnerScorePrefix + winner.Score;

            _exitButton.onClick.AddListener(Exit);
            
            Time.timeScale = 0.0f;
            
            return this;
        }

        private void Exit()
        {
            Time.timeScale = 1.0f;
            _networkObject.Despawn(true);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(Exit);
        }
    }
}