using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    private float gravity = -9.81f;

    private Transform mainCamera;
    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 direction;
    private Vector3 moveDirection;
    private void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        InputHandler.OnMove += MovePlayer;
    }
    private void OnDisable()
    {
        InputHandler.OnMove -= MovePlayer;
    }
    private void Update()
    {
        moveDirection = (mainCamera.right * direction.x + mainCamera.forward * direction.y).normalized;
        moveDirection.y = 0;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void MovePlayer(Vector2 direction)
    {
        this.direction = direction;
    }
}