using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnControllerGame : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {

        if (!NetworkManager.Singleton.IsServer) return;


        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnSceneLoaded;
    }

    private void OnSceneLoaded(string sceneName, LoadSceneMode mode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        if (sceneName != "Game") return;

        int index = 0;

        foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
        {
            NetworkObject playerObject = kvp.Value.PlayerObject;
            if (playerObject == null) continue;

            Transform point = spawnPoints[index % spawnPoints.Length];

            var cc = playerObject.GetComponent<CharacterController>();

            if (cc != null) cc.enabled = false;

            playerObject.transform.position = point.position;
            playerObject.transform.rotation = point.rotation;


            if (cc != null) cc.enabled = true;

            index++;
        }

        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnSceneLoaded;
    }
}
