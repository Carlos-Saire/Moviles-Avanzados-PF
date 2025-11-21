using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using System;
using System.Threading.Tasks;

public class AuthenticationManager : MonoBehaviour
{
    private PlayerInfo playerInfo;
    [SerializeField] private PlayerInfoSO playerSo;

    public static event Action OnSignIn;
    public static event Action OnLogout;

    public static event Action OnDeleteAccount;

    public static event Action<string> OnNameUpdated;
    public static event Action OnPlayerSignedIn;

    private void Reset()
    {
        gameObject.name = "AuthenticationManager";
    }
    private async void Awake()

    {
        await UnityServices.InitializeAsync();

        //if (UnityServices.State == ServicesInitializationState.Initialized)
        //{

        //    if (AuthenticationService.Instance.IsSignedIn)
        //    {
        //        Debug.Log("Unity Services ya están inicializados");
        //        OnNameUpdated?.Invoke(playerSo.PlayerName);
        //        Debug.Log("Se restablecio se mando el nombre del Player: " + playerSo.PlayerName);
        //        OnPlayerSignedIn.Invoke();
        //        Debug.Log("El jugador ya ha iniciado sesión anteriormente.");
        //    }
        //}
        //else
        //{
        //    await UnityServices.InitializeAsync();
        //    Debug.Log("Unity Services se inicializaron ahora");
        //}

        //PlayerAccountService.Instance.SignedIn += SignedInWithUnity;
        //PlayerAccountService.Instance.SignedOut += SignedOutWithUnity;
    }


    //private void OnDestroy()
    //{
    //    PlayerAccountService.Instance.SignedIn -= SignedInWithUnity;
    //    PlayerAccountService.Instance.SignedOut -= SignedOutWithUnity;
    //}
    //private void Start()
    //{
    //    if (AuthenticationService.Instance.IsSignedIn)
    //    {
    //        return;
    //    }

    //    try
    //    {
    //        if (AuthenticationService.Instance.SessionTokenExists)
    //        {

    //            InitSignAnomyn();
    //        }
    //        else
    //        {
    //            Debug.Log(" No hay sesión previa guardada, debes iniciar sesión.");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogWarning("No se pudo restaurar la sesión: " + e.Message);
    //    }
    //}

    public async void EditNameAsync(string newName)
    {
        playerSo.PlayerName = await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);
        OnNameUpdated?.Invoke(playerSo.PlayerName);
        Debug.Log("Edit name");
    }
    public async void DeleteAccountAsync()
    {
        await AuthenticationService.Instance.DeleteAccountAsync();
        PlayerAccountService.Instance.SignOut();

        OnDeleteAccount?.Invoke();
        Debug.Log("Account deleted");
    }
    public async void InitSignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
    }
    public void InitSignOut()
    {
        PlayerAccountService.Instance.SignOut();
        OnLogout?.Invoke();

        Debug.Log("Secion Cerrada");
    }
    public async void InitSignAnomyn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        LoadPlayerInfoAsync();
    }

    private async void SignedInWithUnity()
    {
        try
        {
            string accessToken = PlayerAccountService.Instance.AccessToken;
            Debug.Log("Access Token: " + accessToken);
            await SignInWithUnityAsync(accessToken);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void SignedOutWithUnity()
    {
        AuthenticationService.Instance.SignOut(true);
        Debug.Log("Se borra el cache del player");

        AuthenticationService.Instance.ClearSessionToken();
        Debug.Log("Se borra el Token del player");

        Debug.Log("Se Cerro la secion correctamente");
    }

    private async Task SignInWithUnityAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("SignIn successfully.");

            LoadPlayerInfoAsync();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private async void LoadPlayerInfoAsync()
    {
        try
        {
            playerInfo = AuthenticationService.Instance.PlayerInfo;
            Debug.Log("Player ID: " + playerInfo.Username);

            playerSo.PlayerID = playerInfo.Id;
            Debug.Log("Se mando el player Id al scriptable object");

            playerSo.PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            Debug.Log("Se optuvo nombre del player");

            OnSignIn?.Invoke();
            Debug.Log("Se lanza el evento con valor: " + playerSo.PlayerName);
        }
        catch(Exception e)
        {
            Debug.LogWarning("No se pudo cargar la información del jugador: " + e.Message);
        }
        
    }
    void CheckStates()
    {
        // this is true if the access token exists, but it can be expired or refreshing
        Debug.Log($"Is SignedIn: {AuthenticationService.Instance.IsSignedIn}");

        // this is true if the access token exists and is valid/has not expired
        Debug.Log($"Is Authorized: {AuthenticationService.Instance.IsAuthorized}");

        // this is true if the access token exists but has expired
        Debug.Log($"Is Expired: {AuthenticationService.Instance.IsExpired}");

        // this is true if the access token exists but is being refreshed
        //Debug.Log($"Is Refreshing: {AuthenticationService.Instance.IsRefreshing}");
    }
    void RegisterEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"The player has successfully signed in");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log($"The access token was not refreshed and has expired");
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log($"The player has successfully signed out");
        };
    }

}
