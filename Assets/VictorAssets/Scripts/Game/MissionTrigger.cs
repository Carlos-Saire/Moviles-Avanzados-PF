using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public GameObject missionPanel;

    private bool playerInside;
    private PlayerController currentPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerController player))
            return;

        playerInside = true;
        currentPlayer = player;

        if (player.IsOwner)
            player.SetNearMission(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out PlayerController player))
            return;

        playerInside = false;
        currentPlayer = null;

        if (player.IsOwner)
            player.SetNearMission(null);
    }

    public void StartMission(PlayerController player)
    {
        if (!playerInside)
            return;

        missionPanel.SetActive(true);
        player.FreezePlayer(true);

        missionPanel.GetComponentInChildren<TroncoMiniGame>()?.SetPlayer(player);
        missionPanel.GetComponentInChildren<SymbolOrderMiniGame>()?.SetPlayer(player);
    }
}
