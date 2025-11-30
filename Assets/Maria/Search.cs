using UnityEngine;

public class Search : navMeshMovement
{
    [SerializeField] GameObject doubleClon;
    [SerializeField] GameObject conteiner;
    [SerializeField] VisionDetector visionRef;
    [SerializeField] GameObject[] positionRef;
   
    bool canSpawn = false;
    [SerializeField] int maxPlayers;
    int clonsCreated = 0;
    
    private void OnCollisionEnter(Collision collision)
    {

    }
    private void Start()
    {
        maxPlayers = GameManager.Instance.GetPlayersConected();
        Debug.Log("NUMB OF PLAYERS" + maxPlayers);
        for(int i = 0; i< maxPlayers; i++)
        {
            //positionRef[i].transform.parent = conteiner.transform;
            GameObject doublePlayer = Instantiate(doubleClon, positionRef[i].transform.position, Quaternion.identity, conteiner.transform);
          
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // get numbr of players, compare< tthe generate a new one
        if (other.tag == "Player"&&clonsCreated<= maxPlayers )
        {
            Debug.Log("Collide player type");
            Vector3 spawnPos = other.transform.position - other.transform.forward * 6f;
           // GameObject doublePlayer = Instantiate(doubleClon, spawnPos,Quaternion.identity, conteiner.transform);
           // clonsCreated++;
           // doublePlayer.GetComponent<DoppleGanger>().SetTarget(other.gameObject.transform.position);//works
           
        }
    }
    private void Update()
    {
        CallRandomMovement();
    }
}

