using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class RelayManager : MonoBehaviour
    {
        private Role _role = Role.None;
        
        public async Task<string> CreateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                _role = Role.Host;
                
                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetHostRelayData(
                    allocation.RelayServer.IpV4, 
                    (ushort) allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData);

                return joinCode;
            }
            catch (RelayServiceException ex)
            {
                Debug.LogException(ex);
            }

            return null;
        }
        
        public async Task JoinRelay(string joinCode)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                _role = Role.Client;
                
                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetClientRelayData(
                    joinAllocation.RelayServer.IpV4, 
                    (ushort) joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData);
            }
            catch (RelayServiceException ex)
            {
                Debug.LogException(ex);
            }
        }

        public void Launch()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            SceneManager.LoadScene("Game");
        }

        private void OnSceneChanged(Scene old, Scene current)
        {
            if (_role == Role.Host)
            {
                NetworkManager.Singleton.StartHost();
            }
            else if (_role == Role.Client)
            {
                NetworkManager.Singleton.StartClient();
            }
            
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

    }
}