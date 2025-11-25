using TMPro;
using UnityEngine;
public class LobbyVisualizerUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_Text lobbyCodeText;
    private void Reset()
    {
        gameObject.name = "LobbyVisualizerUI";
    }
    private void Start()
    {
        if (LobbyManager.instance.CurrentLobby != null)
        {
            string code = LobbyManager.instance.CurrentLobby.LobbyCode;
            lobbyCodeText.text = code;
        }
        else
        {
            lobbyCodeText.text = "Sin conexión al lobby";
        }
    }
}
