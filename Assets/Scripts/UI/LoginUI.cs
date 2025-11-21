using UnityEngine;
using Command;
public class LoginUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private Transform welcomePanel;

    [Header("Panels")]
    [SerializeField] private Transform panelLogin;
    [SerializeField] private Transform panelEstate;

    [Header("PlayerInfoSO")]
    [SerializeField] private PlayerInfoSO playerInfoSO;

    [Header("AuthenticationManager")]
    [SerializeField] private AuthenticationManager authentication;
    private void Reset()
    {
        gameObject.name = "LoginUI";    
    }
    private void OnEnable()
    {
        AuthenticationManager.OnSignIn += SignIn;
    }
    private void OnDisable()
    {
        AuthenticationManager.OnSignIn -= SignIn;
    }
    public void LoginPress()
    {
        authentication.InitSignIn();
        PressPanel();
    }
    public void AnonimPress()
    {
        authentication.InitSignAnomyn();
        PressPanel();
    }
    private void PressPanel()
    {
        panelEstate.gameObject.SetActive(true);
        panelLogin.gameObject.SetActive(false);
    }
    private async void SignIn()
    {
        bool isFirstTimePlayer =  await CloudSaveManager.Instance.IsFirstTimePlayer(playerInfoSO.PlayerID);
        if (isFirstTimePlayer)
        {
            welcomePanel.gameObject.SetActive(true);
        }
        else
        {
            CommandQueue.Instance.AddCommand(new LoadSceneCommand("Menu"));
        }
    }
}
