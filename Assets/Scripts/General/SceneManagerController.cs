using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    public static event Action<float> OnSceneLoadProgress;
    private void Reset()
    {
        gameObject.name = "SceneManagerController";
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadSceneAsync(string scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene));
    }
    private IEnumerator LoadSceneCoroutine(string scene)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);

        while (!asyncOp.isDone)
        {
            float progress = asyncOp.progress / 0.9f;
            OnSceneLoadProgress?.Invoke(progress);
            yield return null; 
        }
    }
    public void LoadScene(string scene, LoadSceneMode mode)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene,mode);
    }
}
