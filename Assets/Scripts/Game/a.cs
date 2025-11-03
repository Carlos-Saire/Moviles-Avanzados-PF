using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class a : MonoBehaviour
{
    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnDisconnect;
    }

    private void OnDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Fuiste desconectado o expulsado.");
            NetworkManager.Singleton.Shutdown();

            SceneManager.LoadScene("Menu");
        }
    }
}
