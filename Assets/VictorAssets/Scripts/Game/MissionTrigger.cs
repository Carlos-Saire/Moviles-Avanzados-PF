using UnityEngine;

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
                tronco.SetMissionObject(this); // ahora pasamos this
            }

            // SIMBOLOS
            var sym = currentMissionPanel.GetComponentInChildren<SymbolOrderMiniGame>(true);
            if (sym != null)
            {
                sym.SetPlayer(player);
                sym.SetMissionObject(this); // ahora pasamos this
            }

            // LIBROS
            var book = currentMissionPanel.GetComponentInChildren<BookManager>(true);
            if (book != null)
            {
                book.SetPlayer(player);
                book.SetMissionObject(this); // ahora pasamos this
            }
        }
        else
        {
            Debug.LogError($"[MissionTrigger] ERROR: No se pudo obtener el panel para el tipo: {missionType}.");
        }
    }
}
