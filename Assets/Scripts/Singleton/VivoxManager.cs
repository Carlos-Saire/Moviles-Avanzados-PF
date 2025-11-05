using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class VivoxManager : MonoBehaviour
{
    public static VivoxManager instance;
    private string currentChannelName;

    public static event Action<string> OnMessageReceived;

    private void Reset()
    {
        gameObject.name = "VivoxManager";
    }
    private void OnEnable()
    {
        AuthenticationManager.OnSignedIn += LoginToVivoxAsync;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await VivoxService.Instance.InitializeAsync();

        VivoxService.Instance.ChannelMessageReceived += HandleMessageReceived;
    }
    private void OnDestroy()
    {
        VivoxService.Instance.ChannelMessageReceived -= HandleMessageReceived;
    }
    private void HandleMessageReceived(VivoxMessage message)
    {
        OnMessageReceived?.Invoke(message.MessageText);
    }
    public async void LoginToVivoxAsync(string playerName)
    {
        LoginOptions options = new LoginOptions() { 
            DisplayName = playerName,
            EnableTTS = true
        };
        await VivoxService.Instance.LoginAsync(options);
        Debug.Log(playerName);
    }
    public void A(string a)
    {
        JoinGroupChannelAsync(a);
    }
    public void C()
    {
        FetchHistoryAsync();
    }
    public async void FetchHistoryAsync()
    {
        try
        {
            List<VivoxMessage> historyMessages = (await VivoxService.Instance.GetChannelTextMessageHistoryAsync(currentChannelName, 10)).ToList();
            historyMessages.Reverse();

            foreach (VivoxMessage message in historyMessages)
            {
                Debug.Log($"{message.SenderDisplayName}: {message.MessageText}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching history: {e}");
        }
    }
    public void B(string b)
    {
        SendMessageToChannel(b);
    }
    public async void JoinGroupChannelAsync(string channelToJoin)
    {
        try
        {
            await VivoxService.Instance.JoinGroupChannelAsync(channelToJoin, ChatCapability.TextAndAudio);
            SetMicrophone(false);
            currentChannelName = channelToJoin;
            Debug.Log("Entraste");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    public async void SendMessageToChannel(string message)
    {
        try
        {
            await VivoxService.Instance.SendChannelTextMessageAsync(currentChannelName, message);
            Debug.Log("Mensaje Con Exito");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            throw;
        }
    }

    public void SetMicrophone(bool value)
    {
        if (value)
        {
            VivoxService.Instance.UnmuteInputDevice();
        }
        else
        {
            VivoxService.Instance.MuteInputDevice();
        }
    }
    public async void LogoutVivoxAsync()
    {
        try
        {
            await VivoxService.Instance.LogoutAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Error al cerrar sesión de Vivox: {ex.Message}");
        }
    }
}


//private async void Awake()
//{


//    await UnityServices.InitializeAsync();
//    AuthenticationService.Instance.ClearSessionToken();

//    await AuthenticationService.Instance.SignInAnonymouslyAsync();

//    await VivoxService.Instance.InitializeAsync();
//    await VivoxService.Instance.LoginAsync();

//}
//public async void CreateVoiceChannel()
//{

//}
//async Task InitVivox()
//{
//    await VivoxService.Instance.InitializeAsync();
//    await VivoxService.Instance.LoginAsync(new LoginOptions { DisplayName = "Usuario1" });

//    VivoxService.Instance.ParticipantAddedToChannel += OnParticipantAdded;
//    VivoxService.Instance.ParticipantRemovedFromChannel += OnParticipantRemoved;
//}

//private void OnParticipantRemoved(VivoxParticipant participant)
//{
//    VivoxService.Instance.LoginAsync();
//}

//private void OnParticipantAdded(VivoxParticipant participant)
//{

//}
//public async void LoginToVivoxAsync(string a)
//{
//    LoginOptions options = new LoginOptions();

//    options.DisplayName = a;
//    options.EnableTTS = true;

//    await VivoxService.Instance.LoginAsync(options);
//}
//public void A()
//{
//    JoinGroupChannelAsync("a");
//}
//public async void JoinGroupChannelAsync(string channelToJoin)
//{

//}
//public async void LeaveEchoChannelAsync()
//{
//    string channelToLeave = "Lobby";
//    await VivoxService.Instance.LeaveChannelAsync(channelToLeave);
//}
//}
