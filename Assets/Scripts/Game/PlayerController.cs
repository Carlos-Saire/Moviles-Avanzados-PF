using Unity.Netcode;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    [Header("Components")]
    private Rigidbody rb;

    private Vector2 direction;
    
    private void OnEnable()
    {
        InputHandler.OnMove += Move;
    }
    private void OnDisable()
    {
        InputHandler.OnMove -= Move;
    }
    private void Reset()
    {
        
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;

        MoveRpc(direction);
    }
    private void Move(Vector2 direction)
    {
        this.direction = direction;
    }
    #region Server
    [Rpc(SendTo.Server)]
    private void MoveRpc(Vector3 direction)
    {
        rb.linearVelocity = new Vector3(direction.x, rb.linearVelocity.y, direction.z);
    }
    #endregion
}
