using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DoppleHealth : NetworkBehaviour
{
    public NetworkVariable<bool> IsDead = new NetworkVariable<bool>(false);

    private Animator animator;
    private DoppleGanger dopple;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        dopple = GetComponent<DoppleGanger>();
    }

    [Rpc(SendTo.Server)]
    public void KillServerRpc()
    {
        if (IsDead.Value) return;
        IsDead.Value = true;

        KillClientRpc(NetworkObjectId);

        StartCoroutine(DespawnAfterDelay());
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        if (NetworkObject != null && NetworkObject.IsSpawned)
            NetworkObject.Despawn(true);
    }

    [Rpc(SendTo.Everyone)]
    private void KillClientRpc(ulong targetId)
    {
        if (NetworkObjectId != targetId)
            return;

        // Detener movimiento del dopple
        if (dopple != null)
            dopple.enabled = false;

        animator.SetTrigger("Death");
    }
}
