using System;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;
using System.Xml.Linq;

public class AuthenticationManager : MonoBehaviour
{
    public static event Action<string> OnSignedIn;
    public static event Action<string> OnSignedOut;

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
        if (AuthenticationService.Instance.SessionTokenExists)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    Debug.Log("Sesión restaurada automáticamente con token existente.");
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("El token expiró o no es válido, se requiere nuevo login.");
                    Debug.LogException(ex);
                }
            }

            playerInfo = AuthenticationService.Instance.PlayerInfo;
            string name = await AuthenticationService.Instance.GetPlayerNameAsync();
            OnSignedIn?.Invoke(name);
        }
        else
        {
            Debug.Log("No hay sesión previa, se requiere login nuevo");
        }
    }


    private async void SignedInWithUnity()
    {
        try
        {
            string accessToken = PlayerAccountService.Instance.AccessToken;
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
            Debug.Log(accessToken);
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("SignIn successfully.");

            playerInfo = AuthenticationService.Instance.PlayerInfo;

            string name = await AuthenticationService.Instance.GetPlayerNameAsync();

            if(name == null)
            {
                Debug.Log("No tiene nombre");
            }
            else
            {
                Debug.Log(name);
                Debug.Log("Por fin tiene nombre UWU");
            }

            OnSignedIn?.Invoke(name);
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
    public async void EditNameAsync(string newName)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);
    }

}