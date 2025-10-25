using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 10f;
    private Vector2 currentRotation;
    private void OnValidate()
    {
        if (sensitivity < 0) sensitivity = 0;
    }
    private void OnEnable()
    {
        InputHandler.OnLook += HandleLook;
    }
    private void OnDisable()
    {
        InputHandler.OnLook -= HandleLook;
    }
    private void HandleLook(Vector2 direction)
    {
        currentRotation.x += direction.x * sensitivity * Time.deltaTime;
        currentRotation.y -= direction.y * sensitivity * Time.deltaTime;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -60f, 60f);
        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0f);
    }
}
