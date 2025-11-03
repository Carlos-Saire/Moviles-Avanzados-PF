using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(NetworkObject))]
public class UIManager : NetworkBehaviour
{
    [Header("Panels")]
    [SerializeField] private PanelController[] panels;

    [Header("Kick")]
    [SerializeField] private Transform buttonKickPlayer;
    [SerializeField] private RectTransform contentKickPlayer;
    [SerializeField] private RectTransform prefabInformationKick;

    private void Reset()
    {
        gameObject.name = "UIManager";
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
            await LobbyManager.instance.RefreshHostLobby();
            CreatePlayerKick(LobbyManager.instance.hostLobby.Players);
        }
    }
    private void CreatePlayerKick(List<Player> players)
    {
        for(int i = 0; i < players.Count; ++i)
        {
            RectTransform newPrefab = Instantiate(prefabInformationKick);
            newPrefab.SetParent(contentKickPlayer);
            newPrefab.localScale = Vector3.one;
            newPrefab.GetComponent<KickInformation>().SetInformation(players[i],i);
        }
    }

}
