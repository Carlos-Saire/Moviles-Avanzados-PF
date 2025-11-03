using UnityEngine;
using Unity.Services.Lobbies.Models;


public class PanelLobbyList : PanelController
{
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform content;
    private void Reset()
    {
        gameObject.name = "Panel Lobby List";
    }
    private async void Start()
    {
        QueryResponse lobbies = await LobbyManager.instance.ListLobbies();

        for (int i = 0; i < lobbies.Results.Count; ++i)
        {
            // Debug.Log($"Lobby {i + 1}: {lobbies.Results[i].Name} (ID: {lobbies.Results[i].Id})");
            CreateLobby(lobbies.Results[i]);
        }
    }
    private void CreateLobby(Lobby lobby)
    {
        RectTransform newLobby =Instantiate(prefab);
        newLobby.SetParent(content);
        newLobby.localScale = Vector3.one;
        newLobby.GetComponent<LobbyItem>().UpdateInformation(lobby);
    }
}
