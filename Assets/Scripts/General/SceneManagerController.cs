using System;
using System.Collections;
using Unity.Netcode;
using UnityEditor;
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
    public void LoadScene(string scene, LoadSceneMode mode)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene,mode);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;   
#else
    Application.Quit();                    
#endif
    }
}
