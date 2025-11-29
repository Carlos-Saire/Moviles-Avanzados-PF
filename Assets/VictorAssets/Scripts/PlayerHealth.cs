using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<bool> IsDead = new NetworkVariable<bool>(false);

    private Animator animator;
    private PlayerController controller;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<PlayerController>();
    }

    [Rpc(SendTo.Server)]
    public void KillServerRpc()
    {
        if (IsDead.Value) return;
        IsDead.Value = true;

        KillClientRpc(NetworkObjectId);

        StartCoroutine(DespawnAfterDelay());
        Debug.Log("died");
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);

        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void KillClientRpc(ulong targetId)
    {
        if (NetworkObjectId != targetId)
            return;

        controller.enabled = false;

        if (controller.playerCamera != null)
            controller.playerCamera.gameObject.SetActive(false);

        animator.SetTrigger("Death");
    }
}
