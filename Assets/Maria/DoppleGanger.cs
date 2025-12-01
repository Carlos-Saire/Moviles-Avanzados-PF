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
   //public bool walking;
    private void Update()
    {
        Debug.Log( isWalking);
        UpdateAnimation();
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
    private void OnCollisionEnter(Collision collision)
    {
        
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
        if (other.gameObject.tag == "Attack")
        {
            Debug.Log("collision player doppleganger");
            animator.SetTrigger("Attack");
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
       // isWalking = agent.isStopped;
        lastPosition = transform.position;
        
    }
    private void UpdateAnimation()
    {
        //float actualSpeed = ((transform.position - lastPosition).magnitude) / Time.deltaTime;

        //lastPosition = transform.position;

        // walking = actualSpeed > 0.05f;
        if (HasReachedDestination())
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }



        animator.SetBool("isWalking", isWalking);

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

}
