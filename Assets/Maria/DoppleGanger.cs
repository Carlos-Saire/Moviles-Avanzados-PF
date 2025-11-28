using UnityEngine;

public class DoppleGanger: navMeshMovement
{
    public Transform target;

    private VisionDetector playerVision;
    public bool isWatched = false;
    private void Start()
    {
        Debug.Log("dopple created");
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
            Debug.Log("trigger");
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject. tag == "Player")
        {
            //target = other.transform;
            playerVision = collision.gameObject.GetComponent<VisionDetector>();
            if (playerVision != null)
            {
                playerVision.OnDoppleWatched += HandleBeingWatched;
            }
            Debug.Log("collision");
        }
    }
    private void Update()
    {

    }
    public void SetTarget(Vector3 position)
    {
        //target.position = position; 
        //Vector3 offsetPos = target.position - target.forward * 1.5f;
        agent.SetDestination(position);
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
