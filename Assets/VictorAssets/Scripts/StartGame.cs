using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;
using Command;

public class LobbyUIManager : MonoBehaviour
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
        }
    }

    private void StartGame()
    {
        for(int i = 0; i < commands.Count; ++i)
        {
            CommandQueue.Instance.AddCommand(commands[i]);
        }
    }
    public void Configure(List<ICommand> list)
    {
        commands = list;
    }
}
