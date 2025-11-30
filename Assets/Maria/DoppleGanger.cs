using UnityEngine;

public class DoppleGanger: navMeshMovement
{
    public Transform target;

    private VisionDetector playerVision;
    public bool isWatched = false;

    public float fleeDistance = 4f;  // Distancia que intentará retroceder si lo ven

    private void Update()
    {
        if (target == null)
        {
            CallRandomMovement();
            return;
        }

        if (!isWatched)
        {
            // si NO es visto → perseguir normalmente
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            // si es visto → actuar normal y alejarse un poco
            CallRandomMovement();
           // ActNaturalAndRetreat();
        }
    }

    private void ActNaturalAndRetreat()
    {
        // Evita persecución
        agent.isStopped = false;

        // Dirección opuesta al jugador
        Vector3 dir = (transform.position - target.position).normalized;

        // Punto al que intenta ir
        Vector3 retreatPos = transform.position + dir * fleeDistance;

        agent.SetDestination(retreatPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Doppelganger detectó al jugador y establece objetivo de persecución");
            target = other.transform;
            playerVision = other.GetComponent<VisionDetector>();


            if (playerVision != null)
                playerVision.OnDoppleWatched += HandleBeingWatched;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            target = null;
        }
        }
    private void HandleBeingWatched(bool watched)
    {
        isWatched = watched;

        if (watched)
        {
            Debug.Log("Doppelganger fue visto → se hace el loco y retrocede");
        }
        else
        {
            Debug.Log("Jugador ya no lo ve → retoma persecución");
        }
    }

    private void OnDisable()
    {
        if (playerVision != null)
            playerVision.OnDoppleWatched -= HandleBeingWatched;
    }
}
