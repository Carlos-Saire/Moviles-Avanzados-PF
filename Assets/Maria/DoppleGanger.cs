using UnityEngine;

public class DoppleGanger: navMeshMovement
{
    public Transform target;

    private VisionDetector playerVision;
    public bool isWatched = false;

    public float fleeDistance = 4f;  // Distancia que intentará retroceder si lo ven
    private Vector3 lastPosition;
    private Vector3 velocity;
    public float maxSpeed = 3f;
    private Animator animator;

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
        UpdateAnimation();
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
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
    }
    private void UpdateAnimation()
    {
        Vector3 vel = agent.desiredVelocity;   // <-- ESTA ES LA DIFERENCIA
        Vector3 localVel = transform.InverseTransformDirection(vel);

        float x = localVel.x;
        float z = localVel.z;

        // Sensibilidad
        if (Mathf.Abs(z) < 0.05f) z = 0;
        if (Mathf.Abs(x) < 0.05f) x = 0;

        animator.SetFloat("X", x);
        animator.SetFloat("Z", z);

    }

}
