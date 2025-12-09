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
        GameObject currentMissionPanel = MissionPanelManager.Instance.GetPanel(missionType);

        if (currentMissionPanel == null)
        {
            Debug.LogError($"[MissionTrigger] ERROR: No se pudo obtener el panel para el tipo: {missionType}.");
            return;
        }

        currentMissionPanel.SetActive(true);

        player.FreezePlayerSingle(true);

        var cursor = Object.FindFirstObjectByType<UniversalGamepadCursorV2>(FindObjectsInactive.Include);
        if (cursor != null)
        {
            cursor.gameObject.SetActive(true);
            cursor.EnableCursor(true);
        }

        // Tronco
        var tronco = currentMissionPanel.GetComponentInChildren<TroncoMiniGame>(true);
        if (tronco != null)
        {
            tronco.SetPlayer(player);
            tronco.SetMissionObject(this);   // NECESARIO
        }

        // Símbolos
        var sym = currentMissionPanel.GetComponentInChildren<SymbolOrderMiniGame>(true);
        if (sym != null)
        {
            sym.SetPlayer(player);
            sym.SetMissionObject(this);      // opcional si tu minijuego lo usa
        }

        // Libros
        var book = currentMissionPanel.GetComponentInChildren<BookManager>(true);
        if (book != null)
        {
            book.SetPlayer(player);
            book.SetMissionObject(this);     // opcional
        }
    }
    public void CompleteMission(SinglePlayer.PlayerController player)
    {
        // 1. Avisamos al SpawnManager
        MissionSpawnManager.Instance.CompleteMission(this);

        // 2. Ocultar panel
        GameObject currentMissionPanel = MissionPanelManager.Instance.GetPanel(missionType);
        if (currentMissionPanel != null)
            currentMissionPanel.SetActive(false);

        // 3. Restaurar control del jugador
        player.FreezePlayerSingle(false);

        // 4. Desactivar cursor gamepad
        var cursor = Object.FindFirstObjectByType<UniversalGamepadCursorV2>(FindObjectsInactive.Include);
        if (cursor != null)
        {
            cursor.EnableCursor(false);
            cursor.gameObject.SetActive(false);
        }
    }
}
