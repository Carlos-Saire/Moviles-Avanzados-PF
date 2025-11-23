using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkTransport))]
public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Transform model;
    [SerializeField] private Animator animator;
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 direction;
    private Vector3 moveDirection;

    private float inputX;
    private float inputZ;

    public bool IsFrozen { get; private set; } = false;

    private PlayerInteraction playerInteraction;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = transform.GetChild(1);
    }

    private void OnEnable()
    {
        InputHandler.OnMove += HandleMove;
    }

    private void OnDisable()
    {
        InputHandler.OnMove -= HandleMove;
    }

    public void FreezePlayer(bool state)
    {
        IsFrozen = state;

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(!state);

        direction = Vector2.zero;
        inputX = 0;
        inputZ = 0;

        animator.SetFloat("X", 0);
        animator.SetFloat("Z", 0);

        ForceIdleAnimationServerRpc();
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (IsFrozen) return;

        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (camRight * direction.x + camForward * direction.y).normalized;

        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;

        MoveRpc(move, velocity);

        Quaternion newRotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);
        UpdateRotationServerRpc(newRotation);

        MoveAnimationRpc(inputX, inputZ);
    }

    private void HandleMove(Vector2 dir)
    {
        direction = dir;
        inputX = dir.x;
        inputZ = dir.y;
    }

    public void SetNearMission(MissionTrigger mission)
    {
        playerInteraction.SetNearMission(mission);
    }

    [Rpc(SendTo.Server)]
    private void ForceIdleAnimationServerRpc()
    {
        animator.SetFloat("X", 0);
        animator.SetFloat("Z", 0);
        ForceIdleAnimationClientRpc();
    }

    [Rpc(SendTo.NotServer)]
    private void ForceIdleAnimationClientRpc()
    {
        animator.SetFloat("X", 0);
        animator.SetFloat("Z", 0);
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

    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector3 direction, Vector3 velocity)
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

    [Rpc(SendTo.Owner)]
    public void SetCameraStateClientRpc(bool state)
    {
        playerCamera.gameObject.SetActive(state);
    }

    [Rpc(SendTo.Owner)]
    public void SetPositionClientRpc(Vector3 newPosition)
    {
        Debug.Log($"[SetPositionClientRpc] Owner {OwnerClientId} recibido posicion {newPosition}");

        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        transform.position = newPosition;

        if (cc != null)
            StartCoroutine(ReactivateCCNextFrame(cc));
    }

    private IEnumerator ReactivateCCNextFrame(CharacterController cc)
    {
        yield return null;
        cc.enabled = true;
    }

    public Animator GetAnimator() => animator;
    public Vector3 GetCameraForward() => playerCamera.forward;
    public Vector3 GetCameraPosition() => playerCamera.position;
}
