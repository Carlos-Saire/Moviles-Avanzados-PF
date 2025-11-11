using System;
using Unity.Android.Gradle;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(NetworkObject))]
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    [SerializeField] private Transform playerPrefab;

    public static Func<Vector3> OnPositionPlayer;

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
    private void OnClientDisconnected(ulong obj)
    {
        if (obj == NetworkManager.ServerClientId)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("El servidor se ha desconectado.");
        }
    }
}
