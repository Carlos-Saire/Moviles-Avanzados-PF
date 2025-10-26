namespace VictorGame
{
    using System;
    using System.Threading.Tasks;
    using Unity.Services.Authentication;
    using Unity.Services.Authentication.PlayerAccounts;
    using Unity.Services.Core;
    using UnityEngine;
    public class LoginManager : MonoBehaviour
    {
        public static event Action<string> OnLoginSuccess;
        public static event Action<string> OnLoginFailed;
        public static event Action OnLogout;

        private async void Start()
        {
            await InitializeUnityServices();
            PlayerAccountService.Instance.SignedIn += HandleSignedIn;
        }

        private async Task InitializeUnityServices()
        {
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Servicios inicializados correctamente.");
            }
            catch (Exception e)
            {
                OnLoginFailed?.Invoke($"Error al inicializar: {e.Message}");
            }
        }

        public async void Login()
        {
            try
            {
                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (Exception e)
            {
                OnLoginFailed?.Invoke($"Error al iniciar sesión: {e.Message}");
            }
        }

        private async void HandleSignedIn()
        {
            try
            {
                string accessToken = PlayerAccountService.Instance.AccessToken;
                await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);

                string playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                OnLoginSuccess?.Invoke(playerName);
            }
            catch (Exception e)
            {
                OnLoginFailed?.Invoke($"Error al autenticar: {e.Message}");
            }
        }

        public void Logout()
        {
            AuthenticationService.Instance.SignOut();
            PlayerAccountService.Instance.SignOut();
            OnLogout?.Invoke();
        }
    }
}