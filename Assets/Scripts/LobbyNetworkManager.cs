using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyNetworkManager : MonoBehaviour
{
    [SerializeField] private int _maxPlayers = 5;
    [SerializeField] private LobbyForm _lobbyForm; 
    
    private Lobby _lobby;
    private const float LobbyHeartbeatMax = 15.0f;
    private float _lobbyHeartbeatTimer = LobbyHeartbeatMax;
    
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
        if (_lobby != null)
        {
            _lobbyHeartbeatTimer -= Time.deltaTime;

            if (_lobbyHeartbeatTimer <= 0.0f)
            {
                _lobbyHeartbeatTimer = LobbyHeartbeatMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(_lobby.Id);
            }
        }
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async void CreateLobby(string lobbyName)
    {
        try
        {
            _lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, _maxPlayers);
            _lobbyForm.SetResultMessage(string.Format("Room {0} was created", _lobby.Name), Color.green);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
        }
        
    }
    
    private async void JoinLobby(string lobbyName)
    {
        try
        {
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
                await Lobbies.Instance.JoinLobbyByIdAsync(response.Results[0].Id);
                _lobbyForm.SetResultMessage("Successfully joined", Color.green);
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