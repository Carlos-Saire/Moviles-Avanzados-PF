using UnityEngine;

public class LoginSceneInitializer : MonoBehaviour
{
    [SerializeField] private AuthenticationManager authentication;
    private void Reset()
    {
        gameObject.name = "LoginUI";
    }
    
}
