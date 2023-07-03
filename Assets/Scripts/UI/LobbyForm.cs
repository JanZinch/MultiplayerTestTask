using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class LobbyForm : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _newLobbyName;
        [SerializeField] private TMP_InputField _desiredLobbyName;
        
        [SerializeField] private Button _createLobby;
        [SerializeField] private Button _joinTheLobby;

        [SerializeField] private UnityEvent<string> _onCreateClick;
        [SerializeField] private UnityEvent<string> _onJoinClick;
        
        public UnityEvent<string> OnCreateClick => _onCreateClick;
        public UnityEvent<string> OnJoinClick => _onJoinClick;
        
        
        private void OnEnable()
        {
            _createLobby.onClick.AddListener(CreateLobby);
        }

        private void CreateLobby()
        {
            _onCreateClick?.Invoke(_newLobbyName.text);
        }
        
        private void OnDisable()
        {
            _createLobby.onClick.RemoveListener(CreateLobby);
        }
    }
}