using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;
using Command;

public class StartGameHost : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private List<ICommand> commands;

    private void Start()
    {
        startGameButton.gameObject.SetActive(false);

        if (NetworkManager.Singleton.IsHost)
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.onClick.AddListener(StartGame);

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

            UpdateButtonState();
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        UpdateButtonState();
    }

    private void OnClientDisconnected(ulong clientId)
    {
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        bool canStart = NetworkManager.Singleton.ConnectedClients.Count > 1;
        startGameButton.interactable = canStart;
    }

    private void StartGame()
    {
        for (int i = 0; i < commands.Count; ++i)
        {
            CommandQueue.Instance.AddCommand(commands[i]);
        }
    }

    public void Configure(List<ICommand> list)
    {
        commands = list;
    }
}