namespace VictorGame
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Unity.Netcode;
    using Unity.Netcode.Transports.UTP;
    using Unity.Services.Authentication;
    using Unity.Services.Lobbies;
    using Unity.Services.Lobbies.Models;
    using Unity.Services.Relay;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LobbyManagerMyLobby : MonoBehaviour
    {
        private static LobbyManagerMyLobby instance;

        public static Lobby currentLobby;
        private float heartbeatTimer = 15f;

        public static string RelayJoinCode;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            //Si el unity services no se inicio, lo inicio aca
            if (Unity.Services.Core.UnityServices.State != Unity.Services.Core.ServicesInitializationState.Initialized)
            {
                await Unity.Services.Core.UnityServices.InitializeAsync();
            }

            // NO volver a autenticar si ya toy dentro
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sesión anónima iniciada (solo si no existía una sesión previa)");
            }

            // Solo inicia heartbeat si ya estas en lobby
            if (currentLobby != null)
                StartCoroutine(HeartbeatLobbyCoroutine());
        }

        public async Task CreateLobbyAsync(string lobbyName, int maxPlayers)
        {
            try
            {
                var options = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = new Player(AuthenticationService.Instance.PlayerId)
                };

                currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
                Debug.Log($"Lobby creado. Código: {currentLobby.LobbyCode}");

                // guardar relay
                var allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
                RelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                // Actualizar lobby con data de relay
                await LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { "RelayCode", new DataObject(DataObject.VisibilityOptions.Public, RelayJoinCode) }
                    }
                });

                SceneManager.LoadScene("MyLobby");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error creando lobby: {e.Message}");
            }
        }

        public async Task JoinLobbyByCodeAsync(string code)
        {
            try
            {
                // Si ya toy en un lobby no intentes unirte otra vez 
                if (currentLobby != null && currentLobby.LobbyCode == code)
                {
                    Debug.Log("Ya eres miembro de este lobby. Entrando directamente...");
                    SceneManager.LoadScene("MyLobby");
                    return;
                }

                currentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
                Debug.Log($"Unido al lobby: {currentLobby.Name}");

                if (currentLobby.Data.TryGetValue("RelayCode", out var relayData))
                {
                    RelayJoinCode = relayData.Value;
                    await JoinRelayAsync(RelayJoinCode);
                }

                SceneManager.LoadScene("MyLobby");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("already a member"))
                {
                    Debug.LogWarning("Ya eres miembro del lobby, entrando igual...");
                    SceneManager.LoadScene("MyLobby");
                }
                else
                {
                    Debug.LogError($"Error al unirse al lobby: {e.Message}");
                }
            }
        }

        public async Task LeaveLobbyAsync()
        {
            try
            {
                if (currentLobby != null)
                {
                    await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, AuthenticationService.Instance.PlayerId);
                    currentLobby = null;
                }

                SceneManager.LoadScene("MyLogin");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al salir del lobby: {e.Message}");
            }
        }

        private async Task JoinRelayAsync(string joinCode)
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            transport.SetRelayServerData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
        }

        private System.Collections.IEnumerator HeartbeatLobbyCoroutine()
        {
            while (currentLobby != null)
            {
                yield return new WaitForSeconds(heartbeatTimer);
                LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            }
        }
    }
}