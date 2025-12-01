using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class navMeshMovement : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected float range;
    [SerializeField] protected Transform centerPlane;
    protected bool isWalking;

    void Start()
    {
        
    }
    void Update()
    {
        
        //animator.SetBool("isWalking", isWalking);
    }
    bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;   // LLEGÓ
                }
            }
        }
        return false;  // TODAVÍA MOVIÉNDOSE
    }
    protected void CallRandomMovement()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 target;
            if (RandomPoint(centerPlane.position, range, out target))
            {
                Debug.DrawRay(target, Vector2.up, Color.magenta, 0.8f);
            
                agent.SetDestination(target);
                
            }
         
        }
        //RandomPoint();
    }
    private bool RandomPoint(Vector3 center, float range, out Vector3 result )
    {
        Vector3 randomTarget = center + Random.insideUnitSphere *range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomTarget, out hit,0.5f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
   


}

