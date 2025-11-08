using Unity.Services.Vivox;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
public class VivoxManager : MonoBehaviour
{
    private string currentChannelName;
    private string currentChannelId;

    public static event Action<string> OnMessageReceived;
    public static event Func<string> OnCodeloby;

    [SerializeField] private PlayerInfoSO playerInfoSO;

    private void Reset()
    {
        gameObject.name= "VivoxManager";    
    }
    private async void Awake()
    {
        currentChannelId = OnCodeloby?.Invoke();
        Debug.Log("se optuvo el codigo :" + currentChannelId);
        
        try
        {
            await VivoxService.Instance.InitializeAsync();
            Debug.Log("Se inicializar los servicios de vivox");
        }
        catch (Exception e)
        {
            Debug.LogError("No se pudo inciar los servicios de vivox :" + e.Message);
        }

        LoginToVivoxAsync(playerInfoSO.PlayerName);
         
        VivoxService.Instance.ChannelMessageReceived += HandleMessageReceived;

    }
    private void OnDestroy()
    {
        VivoxService.Instance.ChannelMessageReceived -= HandleMessageReceived;
        LogoutVivoxAsync();
    }
    public async void LoginToVivoxAsync(string playerName)
    {
        LoginOptions options = new LoginOptions()
        {
            DisplayName = playerName,
            EnableTTS = true
        };
        await VivoxService.Instance.LoginAsync(options);
        Debug.Log(playerName);

        JoinGroupChannelAsync(currentChannelId);
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

            Debug.Log("Sesión de Vivox cerrada correctamente.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al cerrar sesión de Vivox: {ex.Message}");
        }
    }
    private void HandleMessageReceived(VivoxMessage message)
    {
        OnMessageReceived?.Invoke(message.MessageText);
    }
}
