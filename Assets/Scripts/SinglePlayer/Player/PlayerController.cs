using System;
using UnityEngine;
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
        private void OnEnable()
        {
            InputHandler.OnMove+=Move;
        }
        private void OnDisable()
        {
            InputHandler.OnMove -= Move;
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();    
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main.transform;
        }
        private void Update()
        {
            MovePlayer();
            MoveAnimation();
        }
        private void Move(Vector2 vector2)
        {
            input = vector2;
        }
        private void MovePlayer()
        {
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
    }
}

