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

        private void Awake()
        {
            panelLogin.SetActive(true);
            panelUser.SetActive(false);
            panelEditName.SetActive(false);
        }

        private void OnEnable()
        {
            LoginManager.OnLoginSuccess += HandleLoginSuccess;
            LoginManager.OnLoginFailed += HandleLoginFailed;
            LoginManager.OnLogout += HandleLogout;
            PlayerProfile.OnNameChanged += UpdatePlayerNameUI;

            loginButton.onClick.AddListener(() => loginManager.Login());
            editNameButton.onClick.AddListener(OpenEditNamePanel);
            logoutButton.onClick.AddListener(() => loginManager.Logout());
            applyNameButton.onClick.AddListener(ApplyNewName);
            closeEditPanelButton.onClick.AddListener(CloseEditPanel);
            openPanelCode.onClick.AddListener(OpenInsertCode);
            closeEditPanelCode.onClick.AddListener(CloseCodePanel);
        }

        private void OnDisable()
        {
            LoginManager.OnLoginSuccess -= HandleLoginSuccess;
            LoginManager.OnLoginFailed -= HandleLoginFailed;
            LoginManager.OnLogout -= HandleLogout;
            PlayerProfile.OnNameChanged -= UpdatePlayerNameUI;
        }

        private void HandleLoginSuccess(string playerName)
        {
            statusText.text = "Inicio de sesión exitoso";
            panelLogin.SetActive(false);
            panelUser.SetActive(true);
            playerProfile.SetInitialName(playerName);
        }

        private void HandleLoginFailed(string message)
        {
            statusText.text = message;
        }

        private void HandleLogout()
        {
            panelUser.SetActive(false);
            panelLogin.SetActive(true);
            statusText.text = "Sesión cerrada.";
        }

        private void OpenEditNamePanel()
        {
            panelEditName.SetActive(true);
        }

        private void OpenInsertCode()
        {
            panelInsertCode.SetActive(true); 
        }
        private void ApplyNewName()
        {
            string newName = nameInput.text.Trim();
            if (!string.IsNullOrEmpty(newName))
            {
                playerProfile.UpdatePlayerName(newName);
                CloseEditPanel();
            }
        }

        private void CloseEditPanel()
        {
            panelEditName.SetActive(false);
        }
        private void CloseCodePanel()
        {
            panelInsertCode.SetActive(false);
        }

        private void UpdatePlayerNameUI(string newName)
        {
            playerNameText.text = "Player: " + newName;
        }
    }
}
