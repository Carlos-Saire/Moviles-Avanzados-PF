using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
public class KickInfo : NetworkBehaviour
{
    [SerializeField] private TMP_Text namePlayer;
    [SerializeField] private Button buttonKick;
    private int indexPlayer;
    public void SetInformation(Player player, int index)
    {
        namePlayer.text = player.Data["PlayerName"].Value;
        indexPlayer = index;
        buttonKick.onClick.AddListener(ClickKick);

        buttonKick.gameObject.SetActive(!NetworkManager.Singleton.IsServer);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        buttonKick.onClick.RemoveListener(ClickKick);
    }
    private void ClickKick()
    {
        LobbyManager.instance.KickPlayer(indexPlayer);
    }
}
