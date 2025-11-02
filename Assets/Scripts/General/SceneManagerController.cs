using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    public static event Action OnCompleteLoadScene;
    private void Awake()
    {
        if (NetworkManager.Singleton.SceneManager == null) return;

        NetworkManager.Singleton.SceneManager.OnSceneEvent += HandleSceneEvent;
    }
    private void OnDestroy()
    {
        if (NetworkManager.Singleton.SceneManager == null) return;

        NetworkManager.Singleton.SceneManager.OnSceneEvent -= HandleSceneEvent;
    }

    private void HandleSceneEvent(SceneEvent sceneEvent)
    {
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                Debug.Log("Termino de cargar" + sceneEvent.SceneName + " , " + sceneEvent.ClientId);
                break;


            case SceneEventType.LoadEventCompleted:
                Debug.Log("Todos Terminaron de cargar la escenea" + sceneEvent.SceneName);
                OnCompleteLoadScene?.Invoke();
                break;
            case SceneEventType.UnloadComplete:
                Debug.Log("Termino de descargar" + sceneEvent.SceneName + " , " + sceneEvent.ClientId);
                break;
            default:
                break;
        }
    }

    public void LoadGameScene(string sceneName)
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Solo el host puede cambiar de escena");
            return;
        }
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
