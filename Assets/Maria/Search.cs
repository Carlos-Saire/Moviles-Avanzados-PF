using UnityEngine;

public class Search : navMeshMovement
{
    [SerializeField] GameObject doubleClon;
    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Collide player type");
       
            GameObject doublePlayer = Instantiate(doubleClon);

            Debug.Log("ola ");
        }
    }
}

