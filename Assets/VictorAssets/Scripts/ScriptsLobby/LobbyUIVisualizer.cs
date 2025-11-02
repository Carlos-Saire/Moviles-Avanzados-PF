namespace VictorGame
{
    using UnityEngine;
    using TMPro;
    using Unity.Services.Lobbies.Models;

    public class LobbyUIVisualizer : MonoBehaviour
    {
        [Header("Referencias UI")]
        public TMP_Text lobbyCodeText;

        private void Start()
        {
            if (LobbyManagerMyLobby.currentLobby != null)
            {
                string code = LobbyManagerMyLobby.currentLobby.LobbyCode;
                lobbyCodeText.text = $"Código de la sala: {code}";
            }
            else
            {
                lobbyCodeText.text = "Sin conexión al lobby";
            }
        }
    }
}
