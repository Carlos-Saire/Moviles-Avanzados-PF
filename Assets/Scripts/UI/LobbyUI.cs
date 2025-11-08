using System;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("Lobby List")]
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private Button lobbyListButton;
    [SerializeField] private Transform lobbyListPanel;
    private void Reset()
    {
        gameObject.name = "LobbyUI";
    }
    private void OnEnable()
    {
        lobbyListButton?.onClick.AddListener(HandleButtonLobbyList);
    }
    private void OnDisable()
    {
        lobbyListButton?.onClick.RemoveListener(HandleButtonLobbyList);
    }
    private void CreateLobby(Lobby lobby)
    {
        RectTransform newLobby = Instantiate(prefab);
        newLobby.SetParent(content);
        newLobby.localScale = Vector3.one;
        newLobby.GetComponent<LobbyInfo>().UpdateInformation(lobby);
    }
    private async void HandleButtonLobbyList()
    {
        QueryResponse lobbies = await LobbyManager.instance.ListLobbies();

        for (int i = 0; i < lobbies.Results.Count; ++i)
        {
            CreateLobby(lobbies.Results[i]);
        }

        lobbyListPanel.gameObject.SetActive(true);
    }
}
