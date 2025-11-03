using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class KickInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text namePlayer;
    [SerializeField] private Button buttonKick;
    private int indexPlayer;

    public void SetInformation(Player player, int index)
    {
        namePlayer.text = player.Data["PlayerName"].Value;
        indexPlayer = index;
        buttonKick.onClick.AddListener(ClickKick);
    }
    private void OnDestroy()
    {
        buttonKick.onClick.RemoveListener(ClickKick);
    }
    private void ClickKick()
    {
        LobbyManager.instance.KickPlayer(indexPlayer);
    }
}
