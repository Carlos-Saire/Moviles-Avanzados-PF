using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (player.IsFrozen) return;
        if (isAttacking) return;

        if (player.GetComponent<PlayerHealth>().IsDead.Value)
            return;

        if (SceneManager.GetActiveScene().name != "Game")
            return;

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

    [Rpc(SendTo.Server)]
    private void TryKillPlayerServerRpc(Vector3 camForward, Vector3 camPosition)
    {
        if (SceneManager.GetActiveScene().name != "Game")
            return;

        float attackRange = 2f;
        float attackRadius = 1.2f;

        Collider[] hits = Physics.OverlapSphere(camPosition + camForward * 1f, attackRadius);

        foreach (Collider col in hits)
        {
            if (col.TryGetComponent<PlayerController>(out PlayerController target))
            {
                if (target != player && target.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
                {
                    float dist = Vector3.Distance(player.transform.position, target.transform.position);
                    if (dist <= attackRange)
                    {
                        hp.KillServerRpc();
                        break; 
                    }
                }
            }
        }
    }
}
