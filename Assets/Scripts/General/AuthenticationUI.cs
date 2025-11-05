using System;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button logoutButton;

    [Header("Panels")]
    [SerializeField] private Transform panelLobby;
    [SerializeField] private Transform PanelEditName;

    [Header("Edit Name")]
    [SerializeField] private Button openEditNameButton;
    [SerializeField] private Button closeEditNameButton;
    [SerializeField] private Button confirmNameButton;
    [SerializeField] private TMP_InputField nameInputField;

    [Header("Texts")]
    [SerializeField] private TMP_Text playerNameText;

    private string newName;

    [SerializeField] private AuthenticationManager authenticationManager;
    private void Reset()
    {
        gameObject.name = "AuthenticationUI";
    }

    private void OnEnable()
    {
        loginButton?.onClick.AddListener(LoginButtonPressed);
        logoutButton?.onClick.AddListener(LogOutButtonPressed);

        openEditNameButton?.onClick.AddListener(OpenEditNameButtonButtonPressed);
        closeEditNameButton?.onClick.AddListener(CloseEditNameButtonButtonPressed);
        closeEditNameButton?.onClick.AddListener(ConfirmNameButtonButtonPressed);
        nameInputField.onValueChanged.AddListener(OnNameInputChanged);

        AuthenticationManager.OnSignedIn += LoginController_OnsignedIn;
        AuthenticationManager.OnSignedOut += LoginController_OnsignedOut;
    }

    private void OnDisable()
    {
        loginButton?.onClick.RemoveListener(LoginButtonPressed);
        logoutButton?.onClick.RemoveListener(LogOutButtonPressed);

        openEditNameButton?.onClick.RemoveListener(OpenEditNameButtonButtonPressed);
        closeEditNameButton?.onClick.RemoveListener(CloseEditNameButtonButtonPressed);
        closeEditNameButton?.onClick.RemoveListener(ConfirmNameButtonButtonPressed);
        nameInputField.onValueChanged.RemoveListener(OnNameInputChanged);

        AuthenticationManager.OnSignedIn -= LoginController_OnsignedIn;
    }
    private void LoginController_OnsignedIn(string playerName)
    {
        playerNameText.text = playerName;

        panelLobby.gameObject.SetActive(false);
        Debug.Log("Player Name: " + playerName );
    }

    private async void LoginButtonPressed()
    {
        await authenticationManager.InitSignIn();
    }

    private void LoginController_OnsignedOut(string response)
    {
      
        Debug.Log(response);
    }
    private void OpenEditNameButtonButtonPressed()
    {
        PanelEditName.gameObject.SetActive(true);
    }
    private void CloseEditNameButtonButtonPressed()
    {
        PanelEditName.gameObject.SetActive(false);
    }
    private async void LogOutButtonPressed()
    {
        await authenticationManager.InitSignOut();
    }

    private void ConfirmNameButtonButtonPressed()
    {
        authenticationManager.EditNameAsync(newName);
    }
    private void OnNameInputChanged(string arg0)
    {
        newName = arg0;
    }
}