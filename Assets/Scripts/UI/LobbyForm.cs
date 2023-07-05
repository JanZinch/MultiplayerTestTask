using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class LobbyForm : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _playerName;
        [SerializeField] private TMP_InputField _newLobbyName;
        [SerializeField] private TMP_InputField _existingLobbyName;
        
        [SerializeField] private Button _createLobby;
        [SerializeField] private Button _joinTheLobby;

        [SerializeField] private TextMeshProUGUI _resultMessage;
        
        [SerializeField] private UnityEvent<string, string> _onCreateClick;
        [SerializeField] private UnityEvent<string, string> _onJoinClick;
        
        public UnityEvent<string, string> OnCreateClick => _onCreateClick;
        public UnityEvent<string, string> OnJoinClick => _onJoinClick;

        private void OnEnable()
        {
            _createLobby.onClick.AddListener(CreateLobby);
            _joinTheLobby.onClick.AddListener(JoinLobby);
        }

        private void CreateLobby()
        {
            _onCreateClick?.Invoke(_playerName.text, _newLobbyName.text);
        }
        
        private void JoinLobby()
        {
            _onJoinClick?.Invoke(_playerName.text, _existingLobbyName.text);
        }

        public void SetResultMessage(string text, Color color)
        {
            _resultMessage.text = text;
            _resultMessage.color = color;
        }

        private void OnDisable()
        {
            _createLobby.onClick.RemoveListener(CreateLobby);
            _joinTheLobby.onClick.RemoveListener(JoinLobby);
        }
    }
}