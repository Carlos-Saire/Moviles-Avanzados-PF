using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MissionSpawnManager : MonoBehaviour
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

    private bool UsingNetcode => NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening;

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

    private void Start()
    {
        InitializeMissions();
    }

    public void CleanUpAllActiveMissions()
    {
        if (UsingNetcode && !NetworkManager.Singleton.IsServer)
            return;

        Debug.Log("Limpiando todas las misiones activas...");

        List<NetworkObject> missionsToDestroy = new List<NetworkObject>(activeMissions.Values);

        activeMissions.Clear();
        availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (var netObj in missionsToDestroy)
        {
            if (netObj == null) continue;

            if (UsingNetcode)
                netObj.Despawn(true);
            else
                Destroy(netObj.gameObject);
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
            missionPrefabs.Add(missionPrefabs[Random.Range(0, missionPrefabs.Count)]);

        ShuffleList(missionPrefabs);

        for (int i = 0; i < availableSpawnPoints.Count; i++)
            SpawnMission(missionPrefabs[i], availableSpawnPoints[i]);
    }

    private void SpawnMission(GameObject missionPrefab, Transform spawnPoint)
    {
        GameObject missionInstance = Instantiate(missionPrefab, spawnPoint.position, spawnPoint.rotation);

        NetworkObject netObj = missionInstance.GetComponent<NetworkObject>();

        if (UsingNetcode && netObj != null)
        {
            netObj.Spawn();
            activeMissions.Add(spawnPoint, netObj);
        }
        else
        {
            // local mode: store as null, but keep spawnPoint occupied
            activeMissions.Add(spawnPoint, null);
        }

        Debug.Log($"Misión '{missionPrefab.name}' creada en {spawnPoint.name}");
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void MissionCompleted(GameObject missionGO)
    {
        Transform spawnUsed = null;

        foreach (var pair in activeMissions)
        {
            if (pair.Value != null && pair.Value.gameObject == missionGO)
            {
                spawnUsed = pair.Key;
                break;
            }
            else if (pair.Value == null && missionGO != null)
            {
                // fallback local mode match by position
                if (Vector3.Distance(pair.Key.position, missionGO.transform.position) < 0.5f)
                {
                    spawnUsed = pair.Key;
                    break;
                }
            }
        }

        if (spawnUsed == null)
            return;

        activeMissions.Remove(spawnUsed);
        Destroy(missionGO);

        StartCoroutine(RespawnMissionAfterDelay(spawnUsed));
    }

    private IEnumerator RespawnMissionAfterDelay(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnTime);

        if (repeatableMissionPrefabs.Count == 0)
        {
            Debug.LogWarning("No hay prefabs repetibles.");
            yield break;
        }

        SpawnMission(repeatableMissionPrefabs[Random.Range(0, repeatableMissionPrefabs.Count)], spawnPoint);
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void CompleteMissionServerRpc(NetworkObjectReference missionRef)
    {
        if (!UsingNetcode) return;

        if (missionRef.TryGet(out var netObj))
            MissionCompleted(netObj.gameObject);
    }
}