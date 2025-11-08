using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
public class LobbyInfo : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text lobbyText;
    [SerializeField] private TMP_Text playerCountText;
    [SerializeField] private TMP_Text SecurityLevelText;

    [Header("Button")]
    private Button button;

    private string lobbyID;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(JoinLobby);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(JoinLobby);
    }
    public void UpdateInformation(Lobby lobby)
    {
        lobbyText.text = lobby.Name;
        playerCountText.text = $"{lobby.Players.Count} / {lobby.MaxPlayers}";
        SecurityLevelText.text = lobby.IsPrivate ? "Privado" : "Público";
        lobbyID = lobby.Id;
    }
    private void JoinLobby()
    {
        LobbyManager.instance.JoinLobbyByID(lobbyID);
    }
}
