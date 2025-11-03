using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    [SerializeField] private SceneManagerController SceneManager;
    public Lobby hostLobby;
    public Lobby joinedLobby;
    private string playerName;
    private void Reset()
    {
        gameObject.gameObject.name = "LobbyManager";
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Carlos" + UnityEngine.Random.Range(0, 100);
    }
    public void Create()
    {
        CreateLobby("a", 5);
    }
    public void A()
    {
        ListLobbies();
    }
    public async void CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers,options);
            hostLobby = lobby;
            Debug.Log("Created Lobby " + lobby.Name + " - " + lobby.MaxPlayers +" - " + lobby.LobbyCode);
            NetworkManager.Singleton.StartHost();

            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);

            Debug.Log(" Host iniciado y escena Lobby cargada correctamente");

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
            joinedLobby = lobby;

            NetworkManager.Singleton.StartClient();
            SceneManager.LoadScene("Lobby");

            Debug.Log("Joined Lobby with code " + lobbycode);
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
            JoinLobbyByIdOptions joinLobbyByCodeOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID, joinLobbyByCodeOptions);
            joinedLobby = lobby;

            NetworkManager.Singleton.StartClient();
            SceneManager.LoadScene("Lobby");

            Debug.Log("Joined Lobby with code " + lobbyID);
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
            await LobbyService.Instance.QuickJoinLobbyAsync();
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
                {"PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName) }
            }
        };
    }
    public async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async Task RefreshHostLobby()
    {
        hostLobby = await LobbyService.Instance.GetLobbyAsync(hostLobby.Id);
    }
    public void PrintPlayer(Lobby lobby)
    {

        for(int i = 0; i < lobby.Players.Count; ++i)
        {
            Debug.Log(lobby.Players[i].Id + " - "+ lobby.Players[i].Data["PlayerName"].Value);
        }
    }
}
