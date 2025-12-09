using System.Collections;
using System.Collections.Generic;
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
    // ahora almacenamos la instancia GameObject localmente (null significa libre)
    private Dictionary<Transform, GameObject> activeMissions;

    public static MissionSpawnManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        activeMissions = new Dictionary<Transform, GameObject>();
        availableSpawnPoints = new List<Transform>(spawnPoints);
    }

    private void Start()
    {
        InitializeMissions();
    }

    public void CleanUpAllActiveMissions()
    {
        Debug.Log("Limpiando todas las misiones activas...");

        List<GameObject> missionsToDestroy = new List<GameObject>(activeMissions.Values);

        activeMissions.Clear();
        availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (var go in missionsToDestroy)
        {
            if (go == null) continue;
            Destroy(go);
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

        // si hay más puntos que prefabs, repite alguno al azar
        while (missionPrefabs.Count < availableSpawnPoints.Count)
            missionPrefabs.Add(missionPrefabs[Random.Range(0, missionPrefabs.Count)]);

        ShuffleList(missionPrefabs);

        for (int i = 0; i < availableSpawnPoints.Count; i++)
            SpawnMission(missionPrefabs[i], availableSpawnPoints[i]);
    }

    private void SpawnMission(GameObject missionPrefab, Transform spawnPoint)
    {
        GameObject missionInstance = Instantiate(missionPrefab, spawnPoint.position, spawnPoint.rotation);

        // registra la instancia localmente para saber que el spawnPoint está ocupado
        activeMissions.Add(spawnPoint, missionInstance);

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

    // llamada cuando una misión local se completa (se le pasa el GameObject de la misión)
    public void MissionCompleted(GameObject missionGO)
    {
        if (missionGO == null) return;

        Transform spawnUsed = null;

        foreach (var pair in activeMissions)
        {
            if (pair.Value == missionGO)
            {
                spawnUsed = pair.Key;
                break;
            }
            else if (pair.Value == null)
            {
                // fallback por proximidad (por si algo se desincroniza)
                if (Vector3.Distance(pair.Key.position, missionGO.transform.position) < 0.5f)
                {
                    spawnUsed = pair.Key;
                    break;
                }
            }
        }

        if (spawnUsed == null)
        {
            Debug.LogWarning("No se encontró spawn asociado a la misión completada.");
            Destroy(missionGO);
            return;
        }

        activeMissions.Remove(spawnUsed);
        Destroy(missionGO);

        StartCoroutine(RespawnMissionAfterDelay(spawnUsed));
    }

    private IEnumerator RespawnMissionAfterDelay(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnTime);

        if (repeatableMissionPrefabs == null || repeatableMissionPrefabs.Count == 0)
        {
            Debug.LogWarning("No hay prefabs repetibles.");
            yield break;
        }

        SpawnMission(repeatableMissionPrefabs[Random.Range(0, repeatableMissionPrefabs.Count)], spawnPoint);
    }

    // Nueva API pública más clara para las minis convertidas que reciben la referencia del trigger
    public void CompleteMission(MissionTrigger trigger)
    {
        if (trigger == null)
        {
            Debug.LogWarning("CompleteMission recibió un trigger nulo.");
            return;
        }

        // encuentra el GameObject de la misión en activeMissions por cercanía al trigger
        Transform spawnUsed = null;
        GameObject missionGO = null;

        foreach (var pair in activeMissions)
        {
            if (pair.Value == null) continue;
            if (Vector3.Distance(pair.Key.position, trigger.transform.position) < 1.0f)
            {
                spawnUsed = pair.Key;
                missionGO = pair.Value;
                break;
            }
        }

        if (spawnUsed == null)
        {
            Debug.LogWarning("No se encontró la misión asociada al trigger al completar.");
            return;
        }

        activeMissions.Remove(spawnUsed);
        Destroy(missionGO);

        StartCoroutine(RespawnMissionAfterDelay(spawnUsed));
    }
}
