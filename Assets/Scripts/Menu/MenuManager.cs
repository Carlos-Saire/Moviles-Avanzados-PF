using System;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button buttonlogin;

    private void OnEnable()
    {
        buttonlogin.onClick.AddListener(Login);
    }

    private void OnDisable()
    {
        buttonlogin.onClick.RemoveListener(Login);
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Holaaaaa" + AuthenticationService.Instance.PlayerId);

        };

    }
    private async void Login()
    {
        try
        {
            await PlayerAccountService.Instance.StartSignInAsync();
            Debug.Log("Esperando confirmación de login...");
        }
        catch (Exception e)
        {
            Debug.Log($"Error al iniciar sesión: {e.Message}");
        }
    }

    private void OnSignedIn()
    {
        Debug.Log("Logueado correctamente como: " + AuthenticationService.Instance.PlayerId);
    }
}
