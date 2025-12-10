using UnityEngine;
using UnityEngine.InputSystem;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] private string missionType;

    private PlayerController currentPlayer;
    private void OnEnable()
    {
        TroncoMiniGame.OnMissionCompleted += Complete;
    }
    private void OnDisable()
    {
        TroncoMiniGame.OnMissionCompleted -= Complete;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SinglePlayer.PlayerController>(out var player))
        {
            player.SetNearMission(this);
        }

        if(other.gameObject.CompareTag("Player"))
            currentPlayer = other.GetComponent<PlayerController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SinglePlayer.PlayerController>(out var player))
        {
            player.SetNearMission(null);
        }

        if (other.gameObject.CompareTag("Player"))
            currentPlayer = null;
    }

    public void StartMission(SinglePlayer.PlayerController player)
    {
        player.GetComponentInChildren<UIManager>().Game();
    }
    private void Complete()
    {
        Debug.Log("Se llamo");
    }
}

