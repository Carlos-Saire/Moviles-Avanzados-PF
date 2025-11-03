using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Netcode;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    [SerializeField] private SceneManagerController SceneManager;
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
            CreateLobbyOptions options = new CreateLobbyOptions();

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            Debug.Log("Created Lobby " + lobby.Name + " - " + lobby.MaxPlayers +" - " + lobby.LobbyCode);
            NetworkManager.Singleton.StartHost();

            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);

            Debug.Log(" Host iniciado y escena Lobby cargada correctamente");
        }
        catch(LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }
    public async void JoinLobbyByCode(string lobbycode)
    {
        try
        {
            await LobbyService.Instance.JoinLobbyByCodeAsync(lobbycode);

            NetworkManager.Singleton.StartClient();
            SceneManager.LoadScene("Lobby");

            Debug.Log("Joined Lobby with code " + lobbycode);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
           

            QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);

            for (int i = 0; i < lobbies.Results.Count; ++i)
            {
                Debug.Log($"Lobby {i + 1}: {lobbies.Results[i].Name} (ID: {lobbies.Results[i].Id})");
            }

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

}
