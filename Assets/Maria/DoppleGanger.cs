using UnityEngine;

public class DoppleGanger: navMeshMovement
{
    public Transform target;

    private VisionDetector playerVision;
    private bool isWatched = false;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // get numbr of players, compare< tthe generate a new one
        if (other.tag == "Player")
        {
            target = other.transform;
            playerVision = other.GetComponent<VisionDetector>();
            if (playerVision != null)
            {
                playerVision.OnDoppleWatched += HandleBeingWatched;
            }
        }
        if (other.tag == "Player" && other.gameObject != target.gameObject)
        {
            Debug.Log("escondete");
        }
        if (other.tag == "DoppleGanger")
        {
            Physics.IgnoreCollision(other, GetComponent<Collider>());
        }
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
    private void HandleBeingWatched(bool watched)
    {
        isWatched = watched;
        if (watched)
        {
            Debug.Log("El jugador vio al Doppelganger → detengo IA");
            agent.isStopped = true;
        }
        else
        {
            Debug.Log("Jugador dejó de mirar → continúo persiguiendo");
            agent.isStopped = false;
        }
    }

    private void OnDisable()
    {
        if (playerVision != null)
            playerVision.OnDoppleWatched -= HandleBeingWatched;
    }
}
