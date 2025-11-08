using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkObject))]
public class SpawnController : NetworkBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPointArray;
    [SerializeField] private Queue<Transform> spawnPoints = new Queue<Transform>();
    private void Reset()
    {
        gameObject.name = "SpawnController";
    }
    public Vector3 GetSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points available!");
            return Vector3.zero;
        }
        Transform spawnPoint = spawnPoints.Dequeue();
        spawnPoints.Enqueue(spawnPoint);
        Debug.Log("Se devolvio una posicion");
        return spawnPoint.position;
    }
    private void Start()
    {
        for (int i = 0; i < spawnPointArray.Length; ++i)
        {
            spawnPoints.Enqueue(spawnPointArray[i]);
        }

        if (NetworkManager.Singleton != null&&IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedRpc;
        }

        if (IsServer)
        {
            OnClientConnectedRpc(NetworkManager.Singleton.LocalClientId);
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedRpc;
    }
    private void OnClientConnectedRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            NetworkObject playerNetworkObject = client.PlayerObject;
            Debug.Log("Se encontro al cliente ");

            GameObject playerGO = playerNetworkObject.gameObject;
            playerGO.transform.position = spawnPointArray[0].position;
            Debug.Log("Modificando posicon del cliente");
        }
    }

}
