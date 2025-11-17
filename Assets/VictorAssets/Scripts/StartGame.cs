using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private void Start()
    {
        startGameButton.gameObject.SetActive(false);

        if (NetworkManager.Singleton.IsHost)
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.onClick.AddListener(StartGame);
        }
    }

    private void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Game",LoadSceneMode.Single);
    }
}
