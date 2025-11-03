using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using VictorGame;

public class Panelkick : MonoBehaviour
{
    private Lobby myloby;
    private void Start()
    {

    }
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LobbyManager.instance.PrintPlayer(LobbyManager.instance.hostLobby);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            await LobbyManager.instance.RefreshHostLobby();
        }
    }
}
