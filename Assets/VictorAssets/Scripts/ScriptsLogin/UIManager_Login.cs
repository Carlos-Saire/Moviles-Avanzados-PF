namespace VictorGame
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    public class UIManager_Login : MonoBehaviour
    {
        [Header("Managers")]
        public LoginManager loginManager;
        public PlayerProfile playerProfile;

        [Header("Panels")]
        public GameObject panelLogin;
        public GameObject panelUser;
        public GameObject panelEditName; 
        public GameObject panelInsertCode; 

        [Header("Login UI")]
        public Button loginButton;
        public TMP_Text statusText;

        [Header("User UI")]
        public TMP_Text playerNameText;
        public Button editNameButton;
        public Button logoutButton;

        [Header("Edit Name UI")]
        public TMP_InputField nameInput;
        public Button applyNameButton;
        public Button closeEditPanelButton;

        [Header("Insert Code")]
        public Button openPanelCode;
        public Button closeEditPanelCode;

        [Header("Lobby Buttons")]
        public Button createLobbyButton;
        public TMP_InputField codeInputField; 
        public Button joinLobbyButton;

        private void Awake()
        {
            ShowOnlyPanel(panelLogin);
        }

        private void OnEnable()
        {
            LoginManager.OnLoginSuccess += HandleLoginSuccess;
            LoginManager.OnLoginFailed += HandleLoginFailed;
            LoginManager.OnLogout += HandleLogout;
            PlayerProfile.OnNameChanged += UpdatePlayerNameUI;

            loginButton.onClick.AddListener(loginManager.Login);
            logoutButton.onClick.AddListener(loginManager.Logout);
            editNameButton.onClick.AddListener(() => ShowPanel(panelEditName, true));
            closeEditPanelButton.onClick.AddListener(() => ShowPanel(panelEditName, false));
            openPanelCode.onClick.AddListener(() => ShowPanel(panelInsertCode, true));
            closeEditPanelCode.onClick.AddListener(() => ShowPanel(panelInsertCode, false));
            applyNameButton.onClick.AddListener(ApplyNewName);

            JoinLobby();
        }

        private void JoinLobby()
        {
            createLobbyButton.onClick.AddListener(() =>
            {
                LobbyManagerMyLobby.IsHost = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("MyLobby");
            });

            joinLobbyButton.onClick.AddListener(() =>
            {
                LobbyManagerMyLobby.IsHost = false;
                LobbyManagerMyLobby.JoinAddress = string.IsNullOrEmpty(codeInputField.text)
                    ? "127.0.0.1"
                    : codeInputField.text.Trim();

                UnityEngine.SceneManagement.SceneManager.LoadScene("MyLobby");
            });
        }

        private void OnDisable()
        {
            LoginManager.OnLoginSuccess -= HandleLoginSuccess;
            LoginManager.OnLoginFailed -= HandleLoginFailed;
            LoginManager.OnLogout -= HandleLogout;
            PlayerProfile.OnNameChanged -= UpdatePlayerNameUI;
        }

        private void ShowPanel(GameObject panel, bool active)
        {
            panel.SetActive(active);
        }

        private void ShowOnlyPanel(GameObject targetPanel)
        {
            panelLogin.SetActive(targetPanel == panelLogin);
            panelUser.SetActive(targetPanel == panelUser);
            panelEditName.SetActive(targetPanel == panelEditName);
            panelInsertCode.SetActive(targetPanel == panelInsertCode);
        }

        private void HandleLoginSuccess(string playerName)
        {
            statusText.text = "Inicio de sesión exitoso";
            ShowOnlyPanel(panelUser);
            playerProfile.SetInitialName(playerName);
        }

        private void HandleLoginFailed(string message)
        {
            statusText.text = message;
        }

        private void HandleLogout()
        {
            ShowOnlyPanel(panelLogin);
            statusText.text = "Sesión cerrada.";
        }

        private void ApplyNewName()
        {
            string newName = nameInput.text.Trim();
            if (!string.IsNullOrEmpty(newName))
            {
                playerProfile.UpdatePlayerName(newName);
                ShowPanel(panelEditName, false);
            }
        }

        private void UpdatePlayerNameUI(string newName)
        {
            playerNameText.text = $"Player: {newName}";
        }
    }
}
