using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

public class AuthenticationUI : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button anomymButton;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerID;
    [SerializeField] private Transform loginPanel;

    [Header("EditName")]
    [SerializeField] private Transform editNamePanel; 
    [SerializeField] private TMP_InputField editNameInputField;
    [SerializeField] private TMP_Text editNameText;
    [SerializeField] private Button confirmsNameButton;
    [SerializeField] private Button openPanelEditNameButton;
    [SerializeField] private Button clousePanelEditNameButton;


    [Header("Logout")]
    [SerializeField] private Button logoutButton;

    [Header("DeleteAccountButton")]
    [SerializeField] private Button deleteAccountButton;

    [Header("PlayerSO")]
    [SerializeField] private PlayerInfoSO playerInfoSO;

    [Header("AuthenticationManager")]
    [SerializeField] private AuthenticationManager authenticationManager;

    private string newName;
    private void Reset()
    {
        gameObject.name = "AuthenticationUI";
    }
    private void OnEnable()
    {
        AuthenticationManager.OnSignIn += HandleOnSignIn;
        AuthenticationManager.OnLogout += HandleOnLogoutAndDeleteAccount;
        AuthenticationManager.OnNameUpdated += HandleOnNameUpdated;
        AuthenticationManager.OnPlayerSignedIn += HandlePlayerSignedIn;
        AuthenticationManager.OnDeleteAccount += HandleOnLogoutAndDeleteAccount;

        loginButton?.onClick.AddListener(HandleloginButton);
        anomymButton?.onClick.AddListener(HandleAnomynButton);
        logoutButton?.onClick.AddListener(authenticationManager.InitSignOut);

        deleteAccountButton?.onClick.AddListener(authenticationManager.DeleteAccountAsync);

        confirmsNameButton?.onClick.AddListener(HandleConfirmsName);
        openPanelEditNameButton?.onClick.AddListener(HandleOpenPanelEditName);
        clousePanelEditNameButton?.onClick.AddListener(HandleClousePanelEditName);
        editNameInputField.onSubmit.AddListener(OnSubmitPlayerName);
        editNameInputField.onValueChanged.AddListener(ValueChangedPlayerName);
    }



    private void OnDisable()
    {
        AuthenticationManager.OnSignIn -= HandleOnSignIn;
        AuthenticationManager.OnLogout -= HandleOnLogoutAndDeleteAccount;
        AuthenticationManager.OnNameUpdated -= HandleOnNameUpdated;
        AuthenticationManager.OnPlayerSignedIn -= HandlePlayerSignedIn;
        AuthenticationManager.OnDeleteAccount -= HandleOnLogoutAndDeleteAccount;

        loginButton?.onClick.RemoveListener(HandleloginButton);
        anomymButton?.onClick.RemoveListener(HandleAnomynButton);
        logoutButton?.onClick.RemoveListener(authenticationManager.InitSignOut);

        deleteAccountButton?.onClick.RemoveListener(authenticationManager.DeleteAccountAsync);

        confirmsNameButton?.onClick.RemoveListener(HandleConfirmsName);
        openPanelEditNameButton?.onClick.RemoveListener(HandleOpenPanelEditName);
        editNameInputField.onValueChanged.RemoveListener(ValueChangedPlayerName);
        editNameInputField.onSubmit.RemoveListener(OnSubmitPlayerName);
        clousePanelEditNameButton?.onClick.RemoveListener(HandleClousePanelEditName);
    }
    private void HandleOnSignIn(string obj)
    {
        loginPanel?.gameObject.SetActive(false);
        Debug.Log("Se desactiva el panel de login");

        playerName.text ="Name : " + obj;
        playerID.text = "ID : "+ playerInfoSO.PlayerID;
        Debug.Log("Se obtuvo El nombre del player: "+obj);
    }
    private void HandleloginButton()
    {
        authenticationManager.InitSignIn();

        Debug.Log("Press Login Button");
    }
    private void HandleOpenPanelEditName()
    {
        editNamePanel.gameObject.SetActive(true);
        editNameText.text = "Ingresa el Nuevo Nombre";

        Debug.Log("Press Open Panel Edit Name Button");
    }

    private void OnSubmitPlayerName(string arg0)
    {
        if (arg0 != "") 
        {
            authenticationManager.EditNameAsync(arg0);
            editNameInputField.text = "";
        }
    }
    private void HandleOnNameUpdated(string newName)
    {
        playerName.text ="Name : " +newName;
        HandleClousePanelEditName();
    }

    private void ValueChangedPlayerName(string arg0)
    {
        string noSpaces = arg0.Replace(" ", "");
        if (noSpaces != arg0)
        {
            editNameInputField.text = noSpaces;
        }
        newName = noSpaces;
    }
    private void HandlePlayerSignedIn()
    {
        loginPanel.gameObject.SetActive(false);
    }
    private void HandleOnLogoutAndDeleteAccount()
    {
        loginPanel.gameObject.SetActive(true);
    }
    private void HandleAnomynButton()
    {
        authenticationManager.InitSignAnomyn();
    }
    private void HandleClousePanelEditName()
    {
        editNamePanel.gameObject.SetActive(false);
    }
    private void HandleConfirmsName()
    {
        authenticationManager.EditNameAsync(newName);
        editNameInputField.text = "";
    }
}
