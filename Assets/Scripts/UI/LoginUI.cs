using UnityEngine;
using Command;
public class LoginUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private Transform panelLogin;
    [SerializeField] private Transform panelEstate;

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
        panelLogin.gameObject.SetActive(false);
        panelEstate.gameObject.SetActive(true);
    }
    private void SignIn()
    {
        CommandQueue.Instance.AddCommand(new LoadSceneCommand("Menu"));
    }
}
