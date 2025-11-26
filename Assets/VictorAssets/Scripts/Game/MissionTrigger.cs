using Unity.Netcode;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] private string missionType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            Debug.Log($"[MissionTrigger] OnTriggerEnter detectado por player Owner={player.OwnerClientId} IsOwner(local)= {player.IsOwner} onClient={NetworkManager.Singleton.LocalClientId}");
            player.SetNearMission(this); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            Debug.Log($"[MissionTrigger] OnTriggerExit detectado por player Owner={player.OwnerClientId} IsOwner(local)= {player.IsOwner} onClient={NetworkManager.Singleton.LocalClientId}");
            player.SetNearMission(null); 
        }
    }

    public void StartMission(PlayerController player)
    {
        Debug.Log("Misión iniciada por " + player.OwnerClientId);

        GameObject currentMissionPanel = null;
        if (MissionPanelManager.Instance != null)
        {
            currentMissionPanel = MissionPanelManager.Instance.GetPanel(missionType);
        }

        if (currentMissionPanel != null)
        {
            currentMissionPanel.SetActive(true);
            player.FreezePlayer(true);

            // TRONCO
            currentMissionPanel.GetComponentInChildren<TroncoMiniGame>(true)?.SetPlayer(player);

            // SIMBOLOS
            currentMissionPanel.GetComponentInChildren<SymbolOrderMiniGame>(true)?.SetPlayer(player);

            // LIBROS 
            currentMissionPanel.GetComponentInChildren<BookManager>(true)?.SetPlayer(player);
        }
        else
        {
            Debug.LogError($"[MissionTrigger] ERROR: No se pudo obtener el panel para el tipo: {missionType}.");
        }
    }
}
