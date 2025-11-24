using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class InstallerLobby : MonoBehaviour
{
    [SerializeField] private LobbyUIManager startGame;

    [SerializeField] private CanvasGroup canvasFade;

    [SerializeField] private string scene;
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Reset()
    {
        gameObject.name = "InstallerLobby";
    }
    private void Start()
    {
        List<ICommand> list = new List<ICommand>()
        {
            new CanvasFadeCommand(canvasFade,1,0.5f),
            new LoadSceneCommand(scene,LoadSceneMode.Single)
        };
        startGame.Configure(list);
    }
}
