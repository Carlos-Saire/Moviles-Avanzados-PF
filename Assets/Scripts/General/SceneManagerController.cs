using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    public static event Action<SceneManagerController> OnSceneLoadProgress;
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
        try
        {
            StartCoroutine(LoadSceneCoroutine(scene));
        }
        catch
        {
            Debug.Log("Igaul fucnionas");
        }
    }
    private IEnumerator LoadSceneCoroutine(string scene)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            Debug.Log($"Progreso: {asyncOp.progress * 100f}%");


            if (asyncOp.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); 
                asyncOp.allowSceneActivation = true;
            }

            yield return null; 
        }
    }
    public void LoadScene(string scene, LoadSceneMode mode)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene,mode);
    }
}
