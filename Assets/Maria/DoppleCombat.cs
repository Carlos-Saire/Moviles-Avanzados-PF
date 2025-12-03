using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoppleCombat : MonoBehaviour
{
    private DoppleGanger dopple;
    private Animator animator;

    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private bool canAttack = true;

    bool attacking = false;

    private void Awake()
    {
        dopple = GetComponent<DoppleGanger>();
        animator = GetComponentInChildren<Animator>();
        Debug.Log("a");
    }
    private void Start()
    {
        Debug.Log("starts");
    }
    private void Update()
    {
        Debug.Log("DoppleCombat Update called");
       // if (!IsServer) return; // IA solo en servidor
        if (dopple.target == null) return;
        Debug.Log("Dopple checking attack conditions");
        if (!dopple.isWatched)
        {
            float dist = Vector3.Distance(transform.position, dopple.target.position);

            if (dist <= attackRange && canAttack)
            {
                Debug.Log("Dopple starts attack");
                attacking = true;
                StartCoroutine(AttackRoutine());
                dopple.target.GetComponent<PlayerHealth2>().IsDead = true;
                Debug.Log("Player dead set to true from DoppleCombat");
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

       animator.SetTrigger("Attack");
         Debug.Log("AttackRoutine IE");
        yield return new WaitForSeconds(0.4f);

        TryKillTarget(); // directo, ya estamos en el server
        Debug.Log("Dopple can attack again");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            //Debug.Log("collision player doppleganger");
           // animator.SetTrigger("Attack");
        }
    }

    private void TryKillTarget()
    {
        if (dopple.target == null) return;

        var targetPlayer = dopple.target.GetComponent<PlayerHealth>();
        if (targetPlayer == null) return;
        if (targetPlayer.IsDead.Value) return;

        targetPlayer.KillServerRpc();
    }
}