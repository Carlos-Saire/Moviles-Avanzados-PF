using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerController player;
    private MissionTrigger nearMission;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        InputHandler.OnInteract += HandleInteract;
    }

    private void OnDisable()
    {
        InputHandler.OnInteract -= HandleInteract;
    }

    public void SetNearMission(MissionTrigger mission)
    {
        nearMission = mission;
    }

    private void HandleInteract()
    {
        if (!player.IsOwner) return;

        if (nearMission != null)
            nearMission.StartMission(player);
    }
}
