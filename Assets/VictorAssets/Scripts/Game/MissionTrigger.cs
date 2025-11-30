using Unity.Netcode;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] private string missionType;
    private NetworkObject missionNetObject;
    private void Awake()
    {
        missionNetObject = GetComponent<NetworkObject>(); 
    }
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
            var tronco = currentMissionPanel.GetComponentInChildren<TroncoMiniGame>(true);
            if (tronco != null)
            {
                tronco.SetPlayer(player);
                tronco.SetMissionObject(missionNetObject);
            }

            // SIMBOLOS
            var sym = currentMissionPanel.GetComponentInChildren<SymbolOrderMiniGame>(true);
            if (sym != null)
            {
                sym.SetPlayer(player);
                sym.SetMissionObject(missionNetObject);
            }

            // LIBROS
            var book = currentMissionPanel.GetComponentInChildren<BookManager>(true);
            if (book != null)
            {
                book.SetPlayer(player);
                book.SetMissionObject(missionNetObject);
            }
        }
        else
        {
            Debug.LogError($"[MissionTrigger] ERROR: No se pudo obtener el panel para el tipo: {missionType}.");
        }
    }
}
