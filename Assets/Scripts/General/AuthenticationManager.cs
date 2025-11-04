using System;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;

public class AuthenticationManager : MonoBehaviour
{
    public event Action<bool, PlayerInfo, string> OnSignedIn;
    public event Action<string> OnSignedOut;
    private PlayerInfo playerInfo;

    [SerializeField] private AuthenticationUI authenticationUI;
    private void Reset()
    {
        gameObject.name = "AuthenticationManager";
    }
    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        PlayerAccountService.Instance.SignedIn += SignedInWithUnity;
        PlayerAccountService.Instance.SignedOut += () => { Debug.Log("--Signed Out"); };
    }
    private void OnDestroy()
    {
        PlayerAccountService.Instance.SignedIn -= SignedInWithUnity;
    }
    private async void Start()
    {

        //if (!AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.SessionTokenExists)
        //{
        //    await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //    Debug.Log("Sesion restaurada automáticamente");
        //}
        //else
        //{
        //    Debug.Log("No hay sesión previa, se requiere login nuevo");
        //}
    }


    private async void SignedInWithUnity()
    {
        try
        {
            var accessToken = PlayerAccountService.Instance.AccessToken;
            await SignInWithUnityAsync(accessToken);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public async Task InitSignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
    }

    public async Task InitSignOut()
    {
        try
        {
            AuthenticationService.Instance.SignOut(true);
            AuthenticationService.Instance.ClearSessionToken();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Signed Out");
            OnSignedOut?.Invoke("Signed Out Completed");
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


    async Task SignInWithUnityAsync(string accessToken)
    {
        try
        {
            bool firstTime = false;
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("SignIn successfully.");

            playerInfo = AuthenticationService.Instance.PlayerInfo;

            var name = await AuthenticationService.Instance.GetPlayerNameAsync(false);

            if (name == null)
            {
                firstTime = true;
                Debug.Log("First time log in");
            }
            else
            {
                Debug.Log("Not first time log in");
            }

            OnSignedIn?.Invoke(firstTime, playerInfo, name);
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

}