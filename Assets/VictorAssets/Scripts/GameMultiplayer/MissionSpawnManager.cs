using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MissionSpawnManager : NetworkBehaviour
{
    [Header("Misiones Prefabs")]
    public GameObject symbolMissionPrefab; 
    public GameObject bookMissionPrefab;   
    public GameObject troncoMissionPrefab; 

    [Header("Puntos de Spawn")]
    public Transform[] spawnPoints;

    [Header("Re-Spawn")]
    public float respawnTime = 10f; 
    public List<GameObject> repeatableMissionPrefabs;

    private List<Transform> availableSpawnPoints;
    private Dictionary<Transform, NetworkObject> activeMissions;

    public static MissionSpawnManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        activeMissions = new Dictionary<Transform, NetworkObject>();

        availableSpawnPoints = new List<Transform>(spawnPoints);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeMissions();
        }
    }
    public void CleanUpAllActiveMissions()
    {
        if (!IsServer) return; 

        Debug.Log("Limpiando todas las misiones activas...");

        List<NetworkObject> missionsToDestroy = new List<NetworkObject>(activeMissions.Values);

        activeMissions.Clear();
        availableSpawnPoints.Clear();

        availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (var netObj in missionsToDestroy)
        {
            if (netObj != null)
            {
                netObj.Despawn(true);
            }
        }
    }
    private void InitializeMissions()
    {
        List<GameObject> missionPrefabs = new List<GameObject>
    {
        symbolMissionPrefab,
        bookMissionPrefab,
        troncoMissionPrefab
    };

        while (missionPrefabs.Count < availableSpawnPoints.Count)
        {
            missionPrefabs.Add(missionPrefabs[Random.Range(0, missionPrefabs.Count)]);
        }

        ShuffleList(missionPrefabs);

        int totalSpawns = availableSpawnPoints.Count;
        for (int i = 0; i < totalSpawns; i++)
        {
            Transform spawnPoint = availableSpawnPoints[0]; 
            availableSpawnPoints.RemoveAt(0); 

            GameObject missionPrefab = missionPrefabs[i];

            SpawnMission(missionPrefab, spawnPoint);
        }
    }
    private void SpawnMission(GameObject missionPrefab, Transform spawnPoint) 
    {
        GameObject missionInstance = Instantiate(missionPrefab, spawnPoint.position, spawnPoint.rotation);

        NetworkObject netObj = missionInstance.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.Spawn();
            activeMissions.Add(spawnPoint, netObj);

            Debug.Log($"Misión '{missionPrefab.name}' spawneada en {spawnPoint.name}");
        }
        else
        {
            Destroy(missionInstance);
            availableSpawnPoints.Add(spawnPoint); 
        }
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }


    public void MissionCompletedAndDestroy(NetworkObject missionNetObj)
    {
        if (!IsServer) return;

        Transform occupiedSpawnPoint = null;
        foreach (var kvp in activeMissions)
        {
            if (kvp.Value == missionNetObj)
            {
                occupiedSpawnPoint = kvp.Key;
                break;
            }
        }

        if (occupiedSpawnPoint != null)
        {
            activeMissions.Remove(occupiedSpawnPoint);

            StartCoroutine(RespawnMissionAfterDelay(occupiedSpawnPoint));
        }

        missionNetObj.Despawn(true);

        Debug.Log($"Misión {missionNetObj.name} completada y destruida en la red.");
    }
    private IEnumerator RespawnMissionAfterDelay(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnTime);

        if (repeatableMissionPrefabs.Count > 0)
        {
            GameObject newMissionPrefab = repeatableMissionPrefabs[Random.Range(0, repeatableMissionPrefabs.Count)];

            SpawnMission(newMissionPrefab, spawnPoint);
        }
        else
        {
            Debug.LogWarning("No hay prefabs en 'Repeatable Mission Prefabs' para re-spawnear.");
        }
    }
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void CompleteMissionServerRpc(NetworkObjectReference missionRef)
    {
        if (!IsServer) return;

        if (missionRef.TryGet(out var netObj))
        {
            MissionCompletedAndDestroy(netObj);
        }
    }
}