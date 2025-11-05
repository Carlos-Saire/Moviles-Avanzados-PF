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
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        TouchSimulation.Enable();
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        RegisterPlayerServerRpc(NetworkManager.Singleton.LocalClientId);

    }

    [Rpc(SendTo.Server)]
    public void RegisterPlayerServerRpc(ulong clientId)
    {
        Transform player = Instantiate(playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetCameraStateClientRpc(true);
        ListPlayer.Add(player);

    }


}