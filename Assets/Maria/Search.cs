using UnityEngine;

public class Search : navMeshMovement
{
    [SerializeField] GameObject doubleClon;
    bool canSpawn = false;
    [SerializeField] int maxPlayers;
    int clonsCreated = 0;
    
    private void OnCollisionEnter(Collision collision)
    {

    }
    private void Start()
    {
        maxPlayers = GameManager.Instance.GetPlayersConected();
        Debug.Log("NUMB" + maxPlayers);
    }
    private void OnTriggerEnter(Collider other)
    {
        // get numbr of players, compare< tthe generate a new one
        if (other.tag == "Player"&&clonsCreated>= maxPlayers )
        {
            Debug.Log("Collide player type");
       
            GameObject doublePlayer = Instantiate(doubleClon,other.transform);
            clonsCreated++;

            Debug.Log("ola ");
        }
    }
}

