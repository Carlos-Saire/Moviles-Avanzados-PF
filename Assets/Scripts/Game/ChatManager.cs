using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
public class ChatManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private void Reset()
    {
        gameObject.name = "ChatManager";
    }
    private void OnEnable()
    {
        inputField.onSubmit.AddListener(HandleMessageSubmit);
    }
    private void OnDisable()
    {
        inputField.onSubmit.RemoveListener(HandleMessageSubmit);
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Unity Services inicializados y login anónimo listo");

        await VivoxService.Instance.InitializeAsync();
        await VivoxService.Instance.LoginAsync();
    }
    private void HandleMessageSubmit(string message)
    {
       
    }
}
