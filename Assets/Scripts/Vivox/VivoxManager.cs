using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;

public class VivoxManager : MonoBehaviour
{
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await VivoxService.Instance.InitializeAsync();
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
}
