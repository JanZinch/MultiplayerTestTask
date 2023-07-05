﻿using System.Collections.Generic;
using UI;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private int _minPlayers = 2;
        [SerializeField] private int _maxPlayers = 5;
        [SerializeField] private LobbyForm _lobbyForm;
        [SerializeField] private RelayManager _relayManager;
        
        private Lobby _lobby;
        
        private const float LobbyHeartbeatMax = 15.0f;
        private float _lobbyHeartbeatTimer = LobbyHeartbeatMax;
        private const float LobbyPollMax = 1.25f;
        private float _lobbyPollTimer = LobbyPollMax;

        private const string StartGameKey = "start_game_code";

        private Role _role = Role.None;
        
        private bool _sessionStarts;
        
        private void OnEnable()
        {
            _lobbyForm.OnCreateClick.AddListener(CreateLobby);
            _lobbyForm.OnJoinClick.AddListener(JoinLobby);
        }

        private async void Start()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed: " + AuthenticationService.Instance.PlayerId);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private async void HandleLobbyHeartbeat()
        {
            if (_role == Role.Host)
            {
                _lobbyHeartbeatTimer -= Time.deltaTime;

                if (_lobbyHeartbeatTimer <= 0.0f)
                {
                    _lobbyHeartbeatTimer = LobbyHeartbeatMax;
                    await LobbyService.Instance.SendHeartbeatPingAsync(_lobby.Id);
                }
            }
        }
        
        private async void HandleLobbyPolling()
        {
            if (_role != Role.None)
            {
                _lobbyPollTimer -= Time.deltaTime;
                
                if (_lobbyPollTimer <= 0.0f)
                {
                    _lobbyPollTimer = LobbyPollMax;

                    Debug.Log("Poll");
                    
                    if (_role == Role.Host)
                    {
                        _lobby = await LobbyService.Instance.GetLobbyAsync(_lobby.Id);

                        if (StartSessionCause(_lobby.Players.Count >= _minPlayers))
                        {
                            string joinCode = await _relayManager.CreateRelay();

                            Debug.Log("Create relay");
                        
                            await Lobbies.Instance.UpdateLobbyAsync(_lobby.Id, new UpdateLobbyOptions()
                            {
                                Data = new Dictionary<string, DataObject>()
                                {
                                    { StartGameKey, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                                }
                            });
                            
                            _relayManager.LaunchRelay();
                        }
                    }
                    else if (_role == Role.Client)
                    {
                        _lobby = await LobbyService.Instance.GetLobbyAsync(_lobby.Id);
                        
                        if (StartSessionCause(_lobby.Data[StartGameKey].Value != string.Empty))
                        {
                            Debug.Log("Try Join Relay");
                            
                            await _relayManager.JoinRelay(_lobby.Data[StartGameKey].Value);
                            
                            _relayManager.LaunchRelay();
                            
                        }
                    }
                }
            }
        }

        private bool StartSessionCause(bool cause)
        {
            if (!_sessionStarts && cause)
            {
                _sessionStarts = true;
                return true;
            }
            else return false;
        }

        private void Update()
        {
            HandleLobbyHeartbeat();
            HandleLobbyPolling();
        }

        private bool VerifyFormInput(string playerName, string lobbyName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                _lobbyForm.SetResultMessage("Input player name", Color.red);
                return false;
            }
            
            if (string.IsNullOrEmpty(lobbyName))
            {
                _lobbyForm.SetResultMessage("Input lobby name", Color.red);
                return false;
            }

            return true;
        }

        private async void CreateLobby(string playerName, string lobbyName)
        {
            try
            {
                if (!VerifyFormInput(playerName, lobbyName)) return;
                
                CreateLobbyOptions createOptions = new CreateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        { StartGameKey, new DataObject(DataObject.VisibilityOptions.Member, string.Empty) }
                    }
                };
                
                _lobbyForm.gameObject.SetActive(false);
                _lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, _maxPlayers, createOptions);
                _role = Role.Host;
                //_lobbyForm.SetResultMessage(string.Format("Room {0} was created", _lobby.Name), Color.green);
                
                PlayerProfilesManager.Instance.InitLocalPlayer(playerName, _role);
                
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogException(ex);
            }
        
        }
    
        private async void JoinLobby(string playerName, string lobbyName)
        {
            try
            {
                if (!VerifyFormInput(playerName, lobbyName)) return;
                
                QueryLobbiesOptions queryOptions = new QueryLobbiesOptions()
                {
                    Count = 1,
                    Filters = new List<QueryFilter>()
                    {
                        new QueryFilter(QueryFilter.FieldOptions.Name, lobbyName, QueryFilter.OpOptions.EQ),
                    }
                };
            
                QueryResponse response = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);

                if (response.Results.Count == 0)
                {
                    _lobbyForm.SetResultMessage("This room don't exists", Color.red);
                }
                else
                {
                    _lobbyForm.gameObject.SetActive(false);
                    _lobby = await Lobbies.Instance.JoinLobbyByIdAsync(response.Results[0].Id);
                    _role = Role.Client;
                    //_lobbyForm.SetResultMessage("Successfully joined", Color.green);
                    
                    PlayerProfilesManager.Instance.InitLocalPlayer(playerName, _role);
                }
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogException(ex);
            }
        
        }
    
        private void OnDisable()
        {
            _lobbyForm.OnCreateClick.RemoveListener(CreateLobby);
            _lobbyForm.OnJoinClick.RemoveListener(JoinLobby);
        }

    }
}