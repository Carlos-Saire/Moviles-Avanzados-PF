using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class KickUI : NetworkBehaviour
{
    [Header("Kick")]
    [SerializeField] private Transform buttonKickPlayer;
    [SerializeField] private RectTransform contentKickPlayer;
    [SerializeField] private RectTransform prefabInformationKick;

    private void Reset()
    {
        gameObject.name = "KickUI";
    }
    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnecte;
    }
    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnecte;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsHost)
        {
            buttonKickPlayer.gameObject.SetActive(true);
            //Debug.Log("Es host");
        }
        else
        {
            buttonKickPlayer.gameObject.SetActive(false);
            //Debug.Log("No es host");
        }
    }

    private async void OnClientConnecte(ulong obj)
    {
        if (IsHost)
        {
            Debug.Log("Cliente conectado, refrescando lista de jugadores");
            await LobbyManager.instance.RefreshLobby();
            CreatePlayerKick(LobbyManager.instance.CurrentLobby.Players);
        }
    }
    private void CreatePlayerKick(List<Player> players)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            RectTransform newPrefab = Instantiate(prefabInformationKick);
            newPrefab.SetParent(contentKickPlayer);
            newPrefab.localScale = Vector3.one;
            newPrefab.GetComponent<KickInfo>().SetInformation(players[i], i);
        }
    }
}
