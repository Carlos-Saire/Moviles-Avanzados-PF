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

            var health = playerObj.GetComponent<PlayerHealth>();
            if (health != null)
                health.ResetPlayer();

            var cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            Transform point = spawnPoints[index % spawnPoints.Length];

            playerObj.transform.position = point.position;
            playerObj.transform.rotation = point.rotation;

            var pc = playerObj.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.SetPositionClientRpc(point.position);
            }

            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                playerObj.transform.position = point.position;
                playerObj.transform.rotation = point.rotation;
            }

            index++;

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
