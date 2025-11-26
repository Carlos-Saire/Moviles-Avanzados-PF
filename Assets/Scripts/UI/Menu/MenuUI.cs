using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Command;
public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] Transform editNamePanel;

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerID;
    [SerializeField] private Image profileImage;

    [Header("PlayerInfoSO")]
    [SerializeField] PlayerInfoSO playerInfoSO;

    private void Reset()
    {
        gameObject.name = "MenuUI";
    }
    private void OnEnable()
    {
        AuthenticationManager.OnNameUpdated += UpdateInfo;
        AuthenticationManager.OnDeleteAccount += DeleteAccount;
        AuthenticationManager.OnLogout += DeleteAccount;
    }
    private void OnDisable()
    {
        AuthenticationManager.OnNameUpdated -= UpdateInfo;
        AuthenticationManager.OnDeleteAccount -= DeleteAccount;
        AuthenticationManager.OnLogout -= DeleteAccount;
    }
    private void Start()
    {
        UpdateInfo();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void UpdateInfo()
    {
        playerName.text = "Name : " + playerInfoSO.PlayerName;
        playerID.text = "ID : " + playerInfoSO.PlayerID;
        profileImage.sprite = playerInfoSO.GetProfileImage();
    }
    public async void AccountButtonPress()
    {
        await CloudSaveManager.Instance.DeleteProfileAsync();
        AuthenticationManager.Instance.DeleteAccountAsync();
    }
    public async void LogoutButtonPress()
    {
        await CloudSaveManager.Instance.SaveProfile();

        AuthenticationManager.Instance.InitSignOut();
    }
    private void DeleteAccount()
    {
        CommandQueue.Instance.AddCommand(new LoadSceneCommand("Login"));
    }
}
