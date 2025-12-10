using System;
using Unity.VisualScripting;
using UnityEngine;
namespace SinglePlayer 
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        public float sensitivity = 2f;

        private float rotationX = 0f;
        private Vector2 inputCamera;

        private InputHandler inputHandler;
        [SerializeField] private Transform playerBody;

        public bool isFrozen = false;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            inputHandler.OnLookSinglePLayer += Move;
        }
        private void OnDestroy()
        {
            inputHandler.OnLookSinglePLayer -= Move;
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            if(!isFrozen)
                RotateCamera();
        }
        private void Move(Vector2 vector)
        {
            inputCamera = vector;
        }
        private void RotateCamera()
        {


            rotationX -= inputCamera.y;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            playerBody.Rotate(Vector3.up * inputCamera.x);
        }
    }

}

