using Unity.Netcode;
using UnityEngine;

public class SpawnController : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        if (NetworkManager.Singleton != null) NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedRpc;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
    }
    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedRpc;
            OnClientConnectedRpc(NetworkManager.Singleton.LocalClientId);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(spawnPoints.Length);
            Debug.Log(spawnPoints[0].name);
        }
    }
    [Rpc(SendTo.Server)]
    private void OnClientConnectedRpc(ulong clientId)
    {

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            NetworkObject playerNetworkObject = client.PlayerObject;

            if (playerNetworkObject != null)
            {
                GameObject playerGO = playerNetworkObject.gameObject;
                Debug.Log(spawnPoints.Length);
                Debug.Log(spawnPoints[0].name);
                playerGO.transform.position = spawnPoints[0].position;
                Debug.Log(spawnPoints[0].name);
            }
            else
            {
                Debug.Log("El PlayerObject aún no está listo");
            }
        }

    }

}
