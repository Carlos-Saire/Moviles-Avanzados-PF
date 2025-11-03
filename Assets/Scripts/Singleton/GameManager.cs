using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Unity.Netcode;
using System.Collections.Generic;
[RequireComponent(typeof(NetworkObject))]
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Transform playerPrefab;
    private List<Transform> ListPlayer = new List<Transform>();
    private void Reset()
    {
        gameObject.name = "GameManager";
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    private void Awake()
    {
        if(Instance == null)
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
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        TouchSimulation.Enable();

        Screen.orientation = ScreenOrientation.AutoRotation;
    }
    private void Practice()
    {
        for(int i = 0; i < ListPlayer.Count; i++)
        {
            ListPlayer[i].gameObject.SetActive(true);
            Debug.Log("Se llamo");
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        RegisterPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }
    private void HandleDisconnect(ulong clientID)
    {
        print("El jugador" + clientID + "Se a desconectado");
    }
    [Rpc(SendTo.Server)]
    public void RegisterPlayerServerRpc(ulong ID)
    {
        Transform player = Instantiate(playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(ID);
        ListPlayer.Add(player);
    }
}
