using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Vivox;
using UnityEngine;

public class VivoxManager : MonoBehaviour
{
    [SerializeField] private Transform a;
    private async void Awake()
    {


        await UnityServices.InitializeAsync();
        //AuthenticationService.Instance.ClearSessionToken();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await VivoxService.Instance.InitializeAsync();
        await VivoxService.Instance.LoginAsync();

    }
    public async void CreateVoiceChannel()
    {

    }
    async Task InitVivox()
    {
        await VivoxService.Instance.InitializeAsync();
        await VivoxService.Instance.LoginAsync(new LoginOptions { DisplayName = "Usuario1" });

        VivoxService.Instance.ParticipantAddedToChannel += OnParticipantAdded;
        VivoxService.Instance.ParticipantRemovedFromChannel += OnParticipantRemoved;
    }

    private void OnParticipantRemoved(VivoxParticipant participant)
    {
        VivoxService.Instance.LoginAsync();
    }

    private void OnParticipantAdded(VivoxParticipant participant)
    {

    }
    public async void LoginToVivoxAsync(string a)
    {
        LoginOptions options = new LoginOptions();

        options.DisplayName = a;
        options.EnableTTS = true;

        await VivoxService.Instance.LoginAsync(options);
    }
    public void A()
    {
        JoinEchoChannelAsync("a");
    }
    public async void JoinEchoChannelAsync(string channelToJoin)
    {
        try
        {
            await VivoxService.Instance.JoinEchoChannelAsync(channelToJoin, ChatCapability.TextAndAudio);
            Debug.Log("Te unite con existo");
            a.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public async void LeaveEchoChannelAsync()
    {
        string channelToLeave = "Lobby";
        await VivoxService.Instance.LeaveChannelAsync(channelToLeave);
    }
}
