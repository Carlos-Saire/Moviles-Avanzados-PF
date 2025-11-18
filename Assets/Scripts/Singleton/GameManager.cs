using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Dictionary<ulong, Transform> connectedPlayers = new Dictionary<ulong, Transform>();

    [Header("Scene Manager")]
    [SerializeField] private SceneManagerController scene;

    [Header("PlayerInfoSo")]
    [SerializeField] private PlayerInfoSO playerInfoSO;
    
    private void Reset()
    {
        gameObject.name = "GameManager";
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadComplete;
        }
    }

   
    public override void OnDestroy()
    {
        base.OnDestroy();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadComplete;
        }
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

        connectedPlayers.Add(clientId, player);
        //Debug.Log(playerInfoSO.PlayerID);
        player.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;   
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetCameraStateClientRpc(true);

    }

    private void OnClientConnected(ulong obj)
    {
        if (IsServer)
        {
            RegisterPlayerServerRpc(obj);
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
    public int CalculatePing()
    {
        int ping = (NetworkManager.Singleton.LocalTime - NetworkManager.Singleton.ServerTime).Tick;
        return ping;
    }
    private void OnLoadComplete(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (!IsServer) return;

        foreach (ulong clientId in clientsCompleted)
        {
            ApplySpawnClientRpc(clientId);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ApplySpawnClientRpc(ulong clientId)
    {
        StartCoroutine(WaitForPlayerAndMove(clientId));
    }

    private IEnumerator WaitForPlayerAndMove(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId != clientId)
            yield break;

        while (NetworkManager.Singleton.LocalClient == null ||
               NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            yield return null;
        }

        var playerObj = NetworkManager.Singleton.LocalClient.PlayerObject;
        var controller = playerObj.GetComponent<PlayerController>();

        if (controller != null)
            controller.enabled = false;

        yield return null;

        Vector3 targetPos = OnPositionPlayer?.Invoke() ?? Vector3.zero;
        playerObj.transform.position = targetPos;

        Debug.Log($"Player {clientId} movido correctamente a {targetPos} después del cambio de escena.");

        yield return null;

        if (controller != null)
            controller.enabled = true;
    }
}

