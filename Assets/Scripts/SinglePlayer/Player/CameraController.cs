using System;
using UnityEngine;
namespace SinglePlayer 
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        public float sensitivity = 2f;

        private float rotationX = 0f;
        [SerializeField] private Transform playerBody;
        private Vector2 inputCamera;
        private void OnEnable()
        {
            InputHandler.OnLook += Move;
        }
        private void OnDisable()
        {
            InputHandler.OnLook -= Move;
        }
        private void Start()
        {
            playerBody = transform.parent;
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
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

