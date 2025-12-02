using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
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
        if (player.IsFrozen) return;
        if (isAttacking) return;

        if (player.GetComponent<PlayerHealth2>().IsDead)
            return;

        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        animator.SetTrigger("Attack");

        if (dagger != null)
            dagger.SetActive(true);

        StartCoroutine(EndAttack());
        TryLocalDamage();
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(1f);

        isAttacking = false;
        animator.SetBool("IsAttacking", false);

        if (dagger != null)
            dagger.SetActive(false);
    }

    private void TryLocalDamage()
    {
        float attackRange = 2f;
        float attackRadius = 1.1f;

        Vector3 pos = player.GetCameraPosition();
        Vector3 forward = player.GetCameraForward();

        Collider[] hits = Physics.OverlapSphere(pos + forward * 1f, attackRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<PlayerController>(out PlayerController target))
            {
                if (target != player)
                {
                    if (target.TryGetComponent<PlayerHealth2>(out PlayerHealth2 hp))
                    {
                        hp.IsDead = true;
                        hp.GetComponentInChildren<Animator>().SetTrigger("Death");
                    }
                }
            }
        }
    }
}
