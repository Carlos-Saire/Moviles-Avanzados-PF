using Unity.Netcode;
using UnityEngine;

public class DoppleCombat : NetworkBehaviour
{
    private DoppleGanger dopple;
    private Animator animator;

    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private bool canAttack = true;


    private void Awake()
    {
        dopple = GetComponent<DoppleGanger>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (dopple.target == null) return;

        // Solo ataca si NO está siendo visto
        if (!dopple.isWatched)
        {
            float dist = Vector3.Distance(transform.position, dopple.target.position);

            if (dist <= attackRange && canAttack)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.4f); // tiempo hasta el daño real

        TryKillTargetServerRpc();

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    [Rpc(SendTo.Server)]
    private void TryKillTargetServerRpc()
    {
        if (dopple.target == null) return;

        var targetPlayer = dopple.target.GetComponent<PlayerHealth>();
        if (targetPlayer != null)
        {
            targetPlayer.KillServerRpc();
        }
    }
}