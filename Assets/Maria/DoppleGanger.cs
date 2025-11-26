using UnityEngine;

public class DoppleGanger: navMeshMovement
{
    public Transform target;

   
    private void Start()
    {
        
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // get numbr of players, compare< tthe generate a new one
        if (other.tag == "Player")
        {
            target = other.transform;
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
}
