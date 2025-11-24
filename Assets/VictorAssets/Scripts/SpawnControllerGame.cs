using UnityEngine;
using Unity.Netcode;

public class SpawnControllerGame : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

        }

        int index = 0;

        foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
        {
            NetworkObject playerObject = kvp.Value.PlayerObject;
            if (playerObject == null) continue;

            Transform point = spawnPoints[index % spawnPoints.Length];
            playerObject.transform.position = point.position;
            playerObject.transform.rotation = point.rotation;

            index++;
        }
    }
}
