using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using System.Collections;
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
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 direction;
    private Vector3 moveDirection;
    private Vector3 move;
    private Quaternion rotation;


    private Transform camera;

    [Header("BT Animator")]
    private float inputX;
    private float inputZ;

    [Header("Attack")]
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private GameObject dagger;
    private void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void OnEnable()
    {
        InputHandler.OnMove += HandleMove;
        InputHandler.OnAttack += HandleAttack;
    }

    private void OnDisable()
    {
        InputHandler.OnMove -= HandleMove;
        InputHandler.OnAttack -= HandleAttack;
    }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = transform.GetChild(1);

        if (dagger != null)
            dagger.SetActive(false);
    }

    private void Update()
    {
        if (!IsOwner) return;
        Move();

        if (isAttacking)
            return;
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;
        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (camRight * direction.x + camForward * direction.y).normalized;
        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
        //controller.Move(move);

        velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);

        MoveRpc(move, velocity);

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
    private void Move()
    {
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;
        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (camRight * direction.x + camForward * direction.y).normalized;
        move = moveDirection * moveSpeed * Time.deltaTime;
        //controller.Move(move);

        velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);
    }
    #region Attack
    private void HandleAttack()
    {
        if (!IsOwner) return;
        if (isAttacking) return;

        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        PlayAttackAnimationRpc();

        if (dagger != null)
            dagger.SetActive(true);

        StartCoroutine(EndAttack());

        TryKillPlayerServerRpc(playerCamera.forward, playerCamera.position);
    }
    [Rpc(SendTo.Server)]
    private void PlayAttackAnimationRpc()
    {
        animator.SetTrigger("Attack");
        PlayAttackAnimationClientRpc();
    }

    [Rpc(SendTo.NotServer)]
    private void PlayAttackAnimationClientRpc()
    {
        animator.SetTrigger("Attack");
    }
    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(1f);

        isAttacking = false;
        animator.SetBool("IsAttacking", false);

        if (dagger != null)
            dagger.SetActive(false);
    }
    #endregion
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
    [Rpc(SendTo.Server)]
    private void TryKillPlayerServerRpc(Vector3 camForward, Vector3 camPosition)
    {
        Debug.Log("SERVER: TryKillPlayerServerRpc ejecutado por " + OwnerClientId);

        float range = 2f;
        float radius = 1f;

        Debug.DrawRay(camPosition, camForward * 2f, Color.red, 2f);

        if (Physics.SphereCast(camPosition, radius, camForward, out RaycastHit hit, range))
        {
            Debug.Log("SERVER: SphereCast impactó con " + hit.collider.name);

            if (hit.collider.TryGetComponent<PlayerController>(out PlayerController target))
            {
                if (target != this && target.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
                {
                    hp.KillServerRpc();
                }
            }
        }
        else
        {
            Debug.Log("SERVER: SphereCast NO impactó con nada");
        }
    }
}