using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUI : MonoBehaviour
{
    public enum ResponseMessageType { successfully, error, }
    [SerializeField] private Button loginButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Transform[] logInPanel, loggedInPanel;
    [SerializeField] private AuthenticationManager authenticationUnityPlayerAccountsControl;
    [SerializeField] private float displayMessageDuration = 10f;
    private void Reset()
    {
        gameObject.name = "AuthenticationUI";
    }

    private void OnEnable()
    {
        loginButton.onClick.AddListener(LoginButtonPressed);
        logoutButton.onClick.AddListener(LogOutButtonPressed);
        authenticationUnityPlayerAccountsControl.OnSignedIn += LoginController_OnsignedIn;
        authenticationUnityPlayerAccountsControl.OnSignedOut += LoginController_OnsignedOut;
    }
    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(LoginButtonPressed);
        logoutButton.onClick.RemoveListener(LogOutButtonPressed);
        authenticationUnityPlayerAccountsControl.OnSignedIn -= LoginController_OnsignedIn;
    }
    private void LoginController_OnsignedIn(bool firstTime, PlayerInfo playerInfo, string playerName)
    {
        foreach (Transform aLogInPanel in logInPanel)
        {
            aLogInPanel.gameObject.SetActive(false);
        }
        foreach (Transform aLoggedInPanel in loggedInPanel)
        {
            aLoggedInPanel.gameObject.SetActive(true);
        }
        Debug.Log("Player Name: " + playerName + " | Player ID: " + playerInfo.Id);
    }

    private async void LoginButtonPressed()
    {
        await authenticationUnityPlayerAccountsControl.InitSignIn();
    }

    private void LoginController_OnsignedOut(string response)
    {
        foreach (Transform aLogInPanel in logInPanel)
        {
            aLogInPanel.gameObject.SetActive(true);
        }
        foreach (Transform aLoggedInPanel in loggedInPanel)
        {
            aLoggedInPanel.gameObject.SetActive(false);
        }
        Debug.Log(response);
    }

    private async void LogOutButtonPressed()
    {
        await authenticationUnityPlayerAccountsControl.InitSignOut();
    }
}