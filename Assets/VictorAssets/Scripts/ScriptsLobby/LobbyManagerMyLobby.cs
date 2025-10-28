namespace VictorGame
{
    using Unity.Netcode;
    using Unity.Netcode.Transports.UTP;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LobbyManagerMyLobby : MonoBehaviour
    {
        public static bool IsHost = false;
        public static string JoinAddress = "127.0.0.1";
        private void Start()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            if (IsHost)
            {
                // El jugador será host
                Debug.Log("Iniciando como HOST...");
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                // El jugador será cliente
                Debug.Log($"Intentando conectarse al host en {JoinAddress}...");
                transport.SetConnectionData(JoinAddress, 7777);
                NetworkManager.Singleton.StartClient();
            }
        }

        public void ExitLobby()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.Shutdown();
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
            }

            SceneManager.LoadScene("MyLogin");
        }
    }
}