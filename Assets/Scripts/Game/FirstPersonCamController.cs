using UnityEngine;
using Unity.Cinemachine;

public class FirstPersonCamController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 10f;
    private Vector2 direction;
    private Vector2 currentRotation;

    [Header("Components")]
    private CinemachineCamera virtualCamera;
    private CinemachinePanTilt panTilt;

    private PlayerController player;
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
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
        panTilt = virtualCamera.GetComponentInChildren<CinemachinePanTilt>();

        player = transform.parent.GetComponent<PlayerController>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void Update()
    {
        MoveLook();
    }
    private void HandleLook(Vector2 direction)
    {
        this.direction = direction;
    }
    private void MoveLook()
    {
        //currentRotation.x += direction.x * sensitivity * Time.deltaTime;
        //currentRotation.y -= direction.y * sensitivity * Time.deltaTime;
        //currentRotation.y = Mathf.Clamp(currentRotation.y, -60f, 60f);
        //panTilt.PanAxis.Value = currentRotation.x;
        //panTilt.TiltAxis.Value = currentRotation.y;


        currentRotation.x += direction.x * sensitivity * Time.deltaTime;
        currentRotation.y -= direction.y * sensitivity * Time.deltaTime;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -60f, 60f);
        panTilt.TiltAxis.Value = currentRotation.y;
        Quaternion newRotation = Quaternion.Euler(0f, currentRotation.x, 0f);
        player.UpdateRotationServerRpc(newRotation);
    }
}