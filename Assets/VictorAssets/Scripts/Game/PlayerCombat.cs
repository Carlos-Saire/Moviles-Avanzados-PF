using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    private PlayerController player;
    private Animator animator;

    private bool isAttacking = false;
    public GameObject dagger;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        animator = player.GetAnimator();
    }

    private void OnEnable()
    {
        InputHandler.OnAttack += HandleAttack;
    }

    private void OnDisable()
    {
        InputHandler.OnAttack -= HandleAttack;
    }

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

        TryKillPlayerServerRpc(player.GetCameraForward(), player.GetCameraPosition());
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

    [Rpc(SendTo.Owner)]
    private void TryKillPlayerServerRpc(Vector3 camForward, Vector3 camPosition)
    {
        float range = 2f;
        float radius = 1f;

        if (Physics.SphereCast(camPosition, radius, camForward, out RaycastHit hit, range))
        {
            if (hit.collider.TryGetComponent<PlayerController>(out PlayerController target))
            {
                if (target != player && target.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
                {
                    hp.KillServerRpc();
                }
            }
        }
    }
}
