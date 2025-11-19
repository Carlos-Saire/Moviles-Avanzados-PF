using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        GameManager.OnPositionPlayer = GetSpawnPoint;

        // Subscribe early pero con check por si NetworkManager no está inicializado al Awake
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnSceneLoaded;
        else
            StartCoroutine(SubscribeWhenReady());
    }

    private IEnumerator SubscribeWhenReady()
    {
        while (NetworkManager.Singleton == null) yield return null;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnSceneLoaded;
    }

    private void OnSceneLoaded(string sceneName, LoadSceneMode mode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (sceneName != "Lobby") return;

        // Asignaremos spawn de forma determinista según clientsCompleted (orden garantizado por el evento)
        int index = 0;

        Debug.Log($"[SpawnController] OnSceneLoaded Lobby. clientsCompleted count = {clientsCompleted.Count}");

        foreach (ulong clientId in clientsCompleted)
        {
            if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var netClient))
            {
                Debug.LogWarning($"[SpawnController] Cliente {clientId} no está en ConnectedClients.");
                continue;
            }

            var playerObj = netClient.PlayerObject;
            if (playerObj == null)
            {
                Debug.LogWarning($"[SpawnController] PlayerObject null para client {clientId}");
                continue;
            }

            var cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            Transform point = spawnPoints[index % spawnPoints.Length];

            // Si el host es este server local, muévelo server-side también para coherencia visual
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                playerObj.transform.position = point.position;
                playerObj.transform.rotation = point.rotation;
            }

            // Llamamos al owner para que él mismo se mueva localmente
            var pc = playerObj.GetComponent<PlayerController>();
            if (pc != null)
            {
                Debug.Log($"[SpawnController] Llamando SetPositionClientRpc a owner {clientId} pos {point.position}");
                pc.SetPositionClientRpc(point.position);
            }
            else
            {
                Debug.LogWarning($"[SpawnController] PlayerController no encontrado en PlayerObject de {clientId}");
            }

            index++;

            // Reactivar CC server-side (por si acaso); la reactivación final en cada cliente la hace su propio RPC
            if (cc != null)
                StartCoroutine(ReenableCC(cc));
        }
    }

    private IEnumerator ReenableCC(CharacterController cc)
    {
        yield return null;
        cc.enabled = true;
    }

    private Vector3 GetSpawnPoint()
    {
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].position;
    }
}
