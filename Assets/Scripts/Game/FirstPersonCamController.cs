using UnityEngine;
using Unity.Cinemachine;
using Unity.Netcode;

public class FirstPersonCamController : NetworkBehaviour
{
    [SerializeField] private float sensitivity = 10f;
    private Vector2 direction;
    private Vector2 currentRotation;

    [Header("Components")]
    private CinemachineCamera virtualCamera;
    private CinemachinePanTilt panTilt;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Start()
    {
        if (!IsOwner)
        {
            if (virtualCamera != null)
                virtualCamera.enabled = false;
        }
        else
        {
            if (virtualCamera != null)
                virtualCamera.enabled = true;
        }
    }
    private void Update()
    {
        MoveLook();
    }
    private void GetTarget(Transform target)
    {
        virtualCamera.Target.TrackingTarget = target;
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
        transform.parent.Rotate(Vector3.up * direction.x * sensitivity * Time.deltaTime);

        currentRotation.y -= direction.y * sensitivity * Time.deltaTime;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -60f, 60f);

        panTilt.TiltAxis.Value = currentRotation.y;
    }
}
