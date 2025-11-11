using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(NetworkObject))]
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    [SerializeField] private Transform playerPrefab;
    public static Func<Vector3> OnPositionPlayer;

    private Dictionary<string, ulong> connectedPlayers = new Dictionary<string, ulong>();

    [Header("Scene Manager")]
    [SerializeField] private SceneManagerController scene;

    [Header("PlayerInfoSo")]
    [SerializeField] private PlayerInfoSO playerInfoSO;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        RegisterPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
        }
    }

    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            Debug.LogWarning($"Cliente {clientId} no encontrado en ConnectedClients");
            return;
        }

        if (client.PlayerObject == null)
        {
            Debug.LogWarning($" El PlayerObject del cliente {clientId} aún no existe. Se intentará reasignar más tarde.");
            return;
        }

        GameObject playerGO = client.PlayerObject.gameObject;
        playerGO.transform.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;
        Debug.Log($" Posición del jugador {clientId} actualizada correctamente");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void Reset()
    {
        gameObject.name = "GameManager";
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Screen.autorotateToPortrait = false;
        //Screen.autorotateToPortraitUpsideDown = false;
        //Screen.autorotateToLandscapeLeft = true;
        //Screen.autorotateToLandscapeRight = true;
        //TouchSimulation.Enable();
        //Screen.orientation = ScreenOrientation.AutoRotation;
    }
    [Rpc(SendTo.Server)]
    public void RegisterPlayerServerRpc(ulong clientId)
    {
        Transform player = Instantiate(playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        //connectedPlayers.Add(playerInfoSO.PlayerID,clientId);
        //Debug.Log(playerInfoSO.PlayerID);
        player.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetCameraStateClientRpc(true);

    }

    private void OnClientConnected(ulong obj)
    {
        Debug.Log("Se llamo");
        
        if (obj == NetworkManager.ServerClientId)
        {
            
            Debug.Log("El servidor se ha conectado.");
        }
    }
    [Rpc(SendTo.Owner)]
    public void DisconnectClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        Debug.Log($"Jugador desconectado por kick.");
    }
    private void OnClientDisconnected(ulong obj)
    {
        if (obj == NetworkManager.ServerClientId)
        {
            scene.LoadScene("Menu");
            NetworkManager.Singleton.Shutdown();
            Debug.Log("El servidor se ha desconectado.");
        }
        if (!IsServer)
        {
            SceneManager.LoadScene("Menu");
            Debug.Log("Se perdio Coneccion con el Server Volviendo al Menu");
        }
    }
}
