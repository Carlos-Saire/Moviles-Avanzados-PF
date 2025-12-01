using System.Collections;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LoadSceneCommand : ICommand
{
    private readonly string _sceneName;
    private readonly LoadSceneMode _loadSceneMode;
    private readonly bool _isNetwork;

    public LoadSceneCommand(string scene)
    {
        _sceneName = scene;
        _loadSceneMode = LoadSceneMode.Single;
        _isNetwork = false;
    }

    public LoadSceneCommand(string scene, LoadSceneMode mode)
    {
        _sceneName = scene;
        _loadSceneMode = mode;
        _isNetwork = true;
    }

    public IEnumerator Execute()
    {
        if (_isNetwork)
        {
            LoadSceneNetwork();
        }
        else
        {
            LoadSceneLocal();
        }

        yield break;
    }

    private void LoadSceneLocal()
    {
        SceneManager.LoadScene(_sceneName, _loadSceneMode);
        InputHandler.IsMove = true;
    }

    private void LoadSceneNetwork()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_sceneName, _loadSceneMode);
        InputHandler.IsMove = true;
    }
}
