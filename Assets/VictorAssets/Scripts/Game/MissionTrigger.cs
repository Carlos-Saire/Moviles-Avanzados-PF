using UnityEngine;
using UnityEngine.InputSystem;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] private string missionType;

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SinglePlayer.PlayerController>(out var player))
        {
            player.SetNearMission(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SinglePlayer.PlayerController>(out var player))
        {
            player.SetNearMission(null);
        }
    }

    public void StartMission(SinglePlayer.PlayerController player)
    {
        player.GetComponentInChildren<UIManager>().Game();
    }
    public void CompleteMission(SinglePlayer.PlayerController player)
    {
        MissionSpawnManager.Instance.CompleteMission(this);

        GameObject currentMissionPanel = MissionPanelManager.Instance.GetPanel(missionType);
        if (currentMissionPanel != null)
            currentMissionPanel.SetActive(false);

        player.FreezePlayerSingle(false);

        var cursor = Object.FindFirstObjectByType<UniversalGamepadCursorV2>(FindObjectsInactive.Include);
        if (cursor != null)
        {
            cursor.EnableCursor(false);
            cursor.gameObject.SetActive(false);
        }
    }
}
