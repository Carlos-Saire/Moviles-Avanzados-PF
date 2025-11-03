using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkTransport))]
[RequireComponent(typeof(NetworkRigidbody))]
public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Transform model; 
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerCamera; 

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 direction;
    private Vector3 moveDirection;

    private void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void OnEnable()
    {
        InputHandler.OnMove += HandleMove;
    }

    private void OnDisable()
    {
        InputHandler.OnMove -= HandleMove;
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = transform.GetChild(1); 
    }

    private void Update()
    {
        if (!IsOwner) return;

        MoveRpc(direction, playerCamera.forward, playerCamera.right);

        UpdateAnimations();
    }

    private void HandleMove(Vector2 direction)
    {
        this.direction = direction;
    }

    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector2 direction, Vector3 camForward, Vector3 camRight)
    {
        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (camRight * direction.x + camForward * direction.y).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            model.rotation = Quaternion.Lerp(model.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        float speed = new Vector2(direction.x, direction.y).magnitude;
        animator.SetFloat("Speed", speed);
    }
}