using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DoppleHealth : MonoBehaviour
{
    public bool IsDead =false;

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
 
        IsDead = true;

       // KillClientRpc(NetworkObjectId);
    Debug.Log("Dopple died");

        StartCoroutine(DespawnAfterDelay());
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        /*if (NetworkObject != null && NetworkObject.IsSpawned)
            NetworkObject.Despawn(true);*/
    }

    private void KillClientRpc()
    {


        // Detener movimiento del dopple
        if (dopple != null)
            dopple.enabled = false;

        animator.SetTrigger("Death");
    }
}
