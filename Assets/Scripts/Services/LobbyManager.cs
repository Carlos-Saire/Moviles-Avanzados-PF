using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Threading.Tasks;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    [Header("PlayerInfoSO")]
    [SerializeField] private PlayerInfoSO playerSO;

    [Header("Lobby")]
    private Lobby currentLobby;

    [Header("Relay")]
    [SerializeField] private RelayManager relayManager;
    private void Reset()
    {
        gameObject.name = "LobbyManager";
    }
    private void OnEnable()
    {
        VivoxManager.OnCodeloby += GetLobbyCode;
    }
    private void OnDisable()
    {
        VivoxManager.OnCodeloby -= GetLobbyCode;
    }

    private string GetLobbyCode()
    {
        return currentLobby.LobbyCode;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (currentLobby != null)
        {
            RemovePlayerAsync();
        }
    }
    public void A()
    {
        ListLobbies();
    }
    public async void CreateLobby(string lobbyName, int maxPlayers,bool isPrivate)
    {
        try
        {
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                Player = GetPlayer(),
            };

            currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);



            string relayJoinCode = await relayManager.CreateRelay(maxPlayers);

            Lobby lobby = await LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                    {
                        { "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                    }
            });

            currentLobby = lobby;

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }
    public async void DeleteLobby(string lobbyName)
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(currentLobby.Name);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }
    public async void JoinLobbyByCode(string lobbycode)
    {
        try
        {

            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbycode, joinLobbyByCodeOptions);
            currentLobby = lobby;

            relayManager.JoinRelay(lobbycode);



        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }
    public async void JoinLobbyByID(string lobbyID)
    {
        try
        {
            Debug.Log(lobbyID);
            JoinLobbyByIdOptions joinLobbyByCodeOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID, joinLobbyByCodeOptions);
            currentLobby = lobby;

            string relayCode = lobby.Data["RelayJoinCode"].Value;

            relayManager.JoinRelay(relayCode);

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }
    public async Task<QueryResponse> ListLobbies()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();


            QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync();


            //for (int i = 0; i < lobbies.Results.Count; ++i)
            //{
            //    Debug.Log($"Lobby {i + 1}: {lobbies.Results[i].Name} (ID: {lobbies.Results[i].Id})");
            //}

            return lobbies;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);

            return null;
        }
    }
    public async void QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            currentLobby = lobby;

            string relayCode = lobby.Data["RelayJoinCode"].Value;
            relayManager.JoinRelay(relayCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public void B(string a)
    {
        JoinLobbyByCode(a);
    }
    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerSO.PlayerName) }
            }
        };
    }
    public async void KickPlayer(int indexPlayer)
    {
        try
        {
            var playerId = currentLobby.Players[indexPlayer].Id;
            await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, currentLobby.Players[indexPlayer].Id);
            GameManager.Instance.DisconnectClientRpc();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void RemovePlayerAsync()
    {
        await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, playerSO.PlayerID);
    }
    public async Task RefreshLobby()
    {
        currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
    }
    public void PrintPlayer(Lobby lobby)
    {

        for (int i = 0; i < lobby.Players.Count; ++i)
        {
            Debug.Log(lobby.Players[i].Id + " - " + lobby.Players[i].Data["PlayerName"].Value);
        }
    }
}
