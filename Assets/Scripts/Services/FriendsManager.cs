using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Friends;
using UnityEngine;

public class FriendsManager : MonoBehaviour
{
    private void Reset()
    {
        gameObject.name = "FriendsManager";
    }
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Se inicializar los servicios de Unity");

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Login Anonimo");

        await FriendsService.Instance.InitializeAsync();
        Debug.Log("Iniciando los servicos de Friends");
    }

    public async void AddFriend(string playerID)
    {
        await FriendsService.Instance.AddFriendAsync(playerID);
    }
    public async void RemoveFriend(string playerID)
    {
        try
        {
            await FriendsService.Instance.DeleteFriendAsync(playerID);

        }
        catch(Exception e) 
        {
            Debug.LogWarning("No se puedo remover el jugador el error es : " + e.Message );
        }
    }
}
