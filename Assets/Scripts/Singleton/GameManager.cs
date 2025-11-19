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
            //NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadComplete;
        }
    }

   
    public override void OnDestroy()
    {
        base.OnDestroy();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            //NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadComplete;
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
        //Transform player = Instantiate(playerPrefab);
        //player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        //connectedPlayers.Add(clientId, player);
        ////Debug.Log(playerInfoSO.PlayerID);
        ////player.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;   COMENTARRRRRRRR
        //PlayerController controller = player.GetComponent<PlayerController>();
        //controller.SetCameraStateClientRpc(true);



        //Transform player = Instantiate(playerPrefab);
        //player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        //connectedPlayers.Add(clientId, player);

        //PlayerController controller = player.GetComponent<PlayerController>();
        //controller.SetCameraStateClientRpc(true);

        //// -------------------------------
        //// SOLO mover si estamos en el LOBBY
        //// -------------------------------
        //if (SceneManager.GetActiveScene().name == "Lobby")
        //{
        //    // Host también necesita posición
        //    if (clientId == NetworkManager.Singleton.LocalClientId)
        //    {
        //        // Aplica spawn del lobby para el host
        //        player.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;
        //    }
        //    else
        //    {
        //        player.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;
        //    }
        //}


        //Transform player;

        //if (clientId == NetworkManager.Singleton.LocalClientId && NetworkManager.Singleton.LocalClient.PlayerObject != null)
        //{
        //    // Host: usar el PlayerObject existente
        //    player = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
        //}
        //else
        //{
        //    // Clientes: instanciar normalmente
        //    player = Instantiate(playerPrefab);
        //    player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        //}

        //if (!connectedPlayers.ContainsKey(clientId))
        //    connectedPlayers.Add(clientId, player);

        //PlayerController controller = player.GetComponent<PlayerController>();
        //controller.SetCameraStateClientRpc(true);

        //// SOLO mover si estamos en el LOBBY
        //if (SceneManager.GetActiveScene().name == "Lobby")
        //{
        //    player.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;
        //}



        Transform player;

        // 1. **Determinar el objeto del jugador (PlayerObject)**
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client) && client.PlayerObject != null)
        {
            // El PlayerObject ya existe (típico para el host si ya estaba en el DontDestroyOnLoad)
            player = client.PlayerObject.transform;
        }
        else
        {
            // El PlayerObject no existe, instanciar y spawnear (típico para clientes o nuevos jugadores)
            player = Instantiate(playerPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }

        // 2. **Registrar el jugador**
        if (!connectedPlayers.ContainsKey(clientId))
            connectedPlayers.Add(clientId, player);

        // 3. **Configurar cámara y otros componentes**
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.SetCameraStateClientRpc(true);
        }

        // 4. **APLICAR POSICIÓN DE SPAWN SOLO EN EL LOBBY**
        // Llama a la lógica de spawn solo si la escena activa es "Lobby".
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            // El operador ?.Invoke() llamará a GetSpawnPoint() de SpawnController.
            // Solo haz esto si el cliente está conectado Y el objeto ya ha sido registrado/creado.
            player.position = OnPositionPlayer?.Invoke() ?? Vector3.zero;
        }
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

    //private void OnLoadComplete(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    //{
    //    if (!IsServer) return;

    //    foreach (ulong clientId in clientsCompleted)
    //    {
    //        ApplySpawnClientRpc(clientId);
    //    }
    //}

    //[Rpc(SendTo.ClientsAndHost)]
    //private void ApplySpawnClientRpc(ulong clientId)
    //{
    //    StartCoroutine(WaitForPlayerAndMove(clientId));
    //}

    //private IEnumerator WaitForPlayerAndMove(ulong clientId)
    //{
    //    if (NetworkManager.Singleton.LocalClientId != clientId)
    //        yield break;

    //    while (NetworkManager.Singleton.LocalClient == null ||
    //           NetworkManager.Singleton.LocalClient.PlayerObject == null)
    //    {
    //        yield return null;
    //    }

    //    var playerObj = NetworkManager.Singleton.LocalClient.PlayerObject;
    //    var controller = playerObj.GetComponent<PlayerController>();

    //    if (controller != null)
    //        controller.enabled = false;

    //    yield return null;

    //    //Vector3 targetPos = OnPositionPlayer?.Invoke() ?? Vector3.zero; COMENTARRRRRR
    //    //playerObj.transform.position = targetPos;

    //    //Debug.Log($"Player {clientId} movido correctamente a {targetPos} después del cambio de escena.");

    //    yield return null;

    //    if (controller != null)
    //        controller.enabled = true;
    //}
}

