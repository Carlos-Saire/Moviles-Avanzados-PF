using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("aea");
    }
    private void Reset()
    {
        gameObject.name = "SceneManager";
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadScene(string scene,LoadSceneMode mode)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene, mode);
    }
}
