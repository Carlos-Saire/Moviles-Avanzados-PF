using Unity.Netcode;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public GameObject missionPanel;

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

        missionPanel.SetActive(true);
        player.FreezePlayer(true);

        // TROCO
        missionPanel.GetComponentInChildren<TroncoMiniGame>(true)?.SetPlayer(player);

        // SIMBOLOS
        missionPanel.GetComponentInChildren<SymbolOrderMiniGame>(true)?.SetPlayer(player);

        // LIBROS 
        missionPanel.GetComponentInChildren<BookManager>(true)?.SetPlayer(player);
    }
}
