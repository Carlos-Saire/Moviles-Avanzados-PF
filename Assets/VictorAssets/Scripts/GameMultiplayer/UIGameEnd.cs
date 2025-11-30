using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameEnd : MonoBehaviour
{
    public static UIGameEnd Instance;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    private void Awake()
    {
        Instance = this;
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    public void Show(bool victory)
    {
        if (victory)
            victoryPanel.SetActive(true);
        else
            defeatPanel.SetActive(true);

        StartCoroutine(ReturnToLobby());
    }

    private System.Collections.IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(4f);

        ReturnToLobbyServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void ReturnToLobbyServerRpc()
    {
        if (MissionSpawnManager.Instance != null)
        {
            MissionSpawnManager.Instance.CleanUpAllActiveMissions();
        }

        NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
