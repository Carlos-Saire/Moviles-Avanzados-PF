using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<bool> IsDead = new NetworkVariable<bool>(false);

    private Animator animator;
    private PlayerController controller;

    private GameObject deathUIPanel;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            deathUIPanel.SetActive(false);
        }
    }

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
        Debug.Log("Player died");
    }

    [Rpc(SendTo.Everyone)]
    private void KillClientRpc(ulong targetId)
    {
        if (NetworkObjectId != targetId)
            return;

        controller.enabled = false;

        if (IsOwner && controller.playerCamera != null)
        {
            controller.playerCamera.gameObject.SetActive(false);

            if (deathUIPanel != null)
                deathUIPanel.SetActive(true);
        }

        animator.SetTrigger("Death");

        if (IsOwner && DeathUIManager.Instance != null)
        {
            DeathUIManager.Instance.ShowDeathScreen();
        }
    }
}
