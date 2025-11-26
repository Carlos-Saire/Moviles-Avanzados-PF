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

        }
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

    }
    [Rpc(SendTo.Server)]
    public void RegisterPlayerServerRpc(ulong clientId)
    {
        Transform player = Instantiate(playerPrefab);

        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Vector3 pos = OnPositionPlayer?.Invoke() ?? new Vector3(0, 5, 0);

        player.position = pos;
        player.rotation = Quaternion.identity;

        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        connectedPlayers[clientId] = player;

        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetCameraStateClientRpc(true);

        if (cc != null) StartCoroutine(ReenableCC(cc));

        Debug.Log($"Player {clientId} instanciado correctamente en {pos}");
   
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
    private IEnumerator ReenableCC(CharacterController cc)
    {
        yield return null;
        cc.enabled = true;
    }
    public int GetPlayersConected()
    {
        return connectedPlayers.Count;
    }
}

