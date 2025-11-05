using DG.Tweening;
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

    [Header("BT Animator")]
    private float inputX;
    private float inputZ;
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



        MoveAnimationRpc(inputX, inputZ);
    }

    private void HandleMove(Vector2 direction)
    {
        this.direction = direction;
        inputX = direction.x;
        inputZ = direction.y;
    }
    [Rpc(SendTo.Server)]
    public void UpdateRotationServerRpc(Quaternion newRotation)
    {
        transform.rotation = newRotation;
    }
    [Rpc(SendTo.Owner)]
    public void SetCameraStateClientRpc(bool state)
    {
        playerCamera.gameObject.SetActive(state);
    }
    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector2 direction, Vector3 camForward, Vector3 camRight)
    {
        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (camRight * direction.x + camForward * direction.y).normalized;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    [Rpc(SendTo.Server)]
    private void MoveAnimationRpc(float inputX, float inputZ)
    {
        animator.SetFloat("X", inputX, 0.1f, Time.deltaTime);
        animator.SetFloat("Z", inputZ, 0.1f, Time.deltaTime);
    }
}