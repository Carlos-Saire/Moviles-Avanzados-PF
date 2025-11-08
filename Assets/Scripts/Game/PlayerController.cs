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
    private Quaternion rotation;

    private Transform camera;

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

        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;
        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (camRight * direction.x + camForward * direction.y).normalized;
        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
        //controller.Move(move);

        velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);

        MoveRpc(move,velocity);

        Quaternion newRotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);
        UpdateRotationServerRpc(newRotation);

        MoveAnimationRpc(inputX, inputZ);
    }
    public void rotate(Quaternion a)
    {
        rotation = a;
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
        model.rotation = newRotation;
        UpdateRotationClientRpc(newRotation);
    }

    [Rpc(SendTo.NotServer)]
    private void UpdateRotationClientRpc(Quaternion newRotation)
    {
        model.rotation = newRotation;
    }
    [Rpc(SendTo.Owner)]
    public void SetCameraStateClientRpc(bool state)
    {
        playerCamera.gameObject.SetActive(state);
    }
    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector3 direction,Vector3 velocity)
    {
        controller.Move(direction);
        controller.Move(velocity);
    }
    [Rpc(SendTo.Server)]
    private void MoveAnimationRpc(float inputX, float inputZ)
    {
        animator.SetFloat("X", inputX, 0.1f, Time.deltaTime);
        animator.SetFloat("Z", inputZ, 0.1f, Time.deltaTime);
    }
}