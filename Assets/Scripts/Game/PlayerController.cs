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
        mainCamera = Camera.main.transform;
    }
    private void Start()
    {
        mainCamera = transform.GetChild(1);
    }
    private void Update()
    {
        if(!IsOwner) return;
        MoveRpc(direction, mainCamera.forward, mainCamera.right);

    }
    private void HandleMove(Vector2 direction)
    {
        this.direction = direction;
    }
    protected override void OnNetworkPostSpawn()
    {
       base.OnNetworkPostSpawn();
    }
    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector2 direction, Vector3 forward , Vector3 right)
    {
        moveDirection = (right * direction.x + forward * direction.y).normalized;
        moveDirection.y = 0;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}