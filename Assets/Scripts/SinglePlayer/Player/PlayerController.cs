using System;
using UnityEngine;
using UnityEngine.Windows;
namespace SinglePlayer
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float gravity = -9.81f;

        private Transform mainCamera;
        private CharacterController controller;
        private Vector3 velocity;
        private Animator animator;
        private Vector2 input;

        private InputHandler inputHandler;
        private PlayerInteraction playerInteraction;

        private bool isInInteractionZone = false;

        private bool isFrozen = false;
        public void FreezePlayerSingle(bool state) 
        {
            isFrozen = state;

            animator.SetFloat("X", 0);
            animator.SetFloat("Z", 0);

            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();    
            controller = GetComponent<CharacterController>();
            mainCamera = GetComponentInChildren<CameraController>().transform; 
            inputHandler = GetComponentInChildren<InputHandler>();
            playerInteraction = GetComponent<PlayerInteraction>();
            mainCamera = mainCamera.transform;
            inputHandler.OnMoveSinglePLayer += Move;
        }
        private void OnDestroy()
        {
            inputHandler.OnMoveSinglePLayer -= Move;
        }
        private void Update()
        {
            MovePlayer();
            MoveAnimation();
        }
        private void Move(Vector2 vector2)
        {
            if (isFrozen)
            {
                input = Vector2.zero;
                return;
            }

            input = vector2;
        }
        private void MovePlayer()
        {
            if (isFrozen) return;

            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            Vector3 direction = (mainCamera.right * input.x + mainCamera.forward * input.y).normalized;
            direction.y = 0;

            controller.Move(direction * moveSpeed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        private void MoveAnimation()
        {
            animator.SetFloat("X", input.x, 0.1f, Time.deltaTime);
            animator.SetFloat("Z", input.y, 0.1f, Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractionZone"))
            {
                isInInteractionZone = true;
                Debug.Log("Entró a la zona de interacción.");
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InteractionZone"))
            {
                isInInteractionZone = false;
                Debug.Log("Salió de la zona de interacción.");
                
            }
        }
        public void SetNearMission(MissionTrigger mission)
        {
            playerInteraction.SetNearMission(mission);
        }
        public void FreezePlayer(bool state)
        {


            animator.SetFloat("X", 0);
            animator.SetFloat("Z", 0);

            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;

        }
    }
}

