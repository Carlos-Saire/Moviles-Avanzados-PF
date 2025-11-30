using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Netcode;
public class SymbolOrderMiniGame : NetworkBehaviour
{
    [Header("Panel de la misión")]
    public GameObject missionPanel;

    [Header("Slots correctos (arriba)")]
    public Image[] correctOrder;

    [Header("Slots del jugador (abajo)")]
    public Transform[] playerSlots;

    [Header("Objetos para Reiniciar")]
    [SerializeField] private GameObject symbolPrefab;
    [SerializeField] private Transform initialSymbolContainer;
    [SerializeField] private Canvas rootCanvas;
    private bool completed = false;

    [Header("Posiciones Iniciales de Símbolos")]
    [SerializeField] private Transform[] initialSymbolPositions;

    private PlayerController currentPlayer;
    private NetworkObject missionObject;
    public void SetMissionObject(NetworkObject missionObj)
    {
        missionObject = missionObj;
    }
    public void SetPlayer(PlayerController player)
    {
        currentPlayer = player;
    }
    private void OnEnable()
    {
        ResetMission();
    }
    public void CheckOrder()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (playerSlots[i].childCount == 0)
            {
                return;
            }

            Image symbolImage = playerSlots[i].GetChild(0).GetComponent<Image>();

            if (symbolImage.sprite != correctOrder[i].sprite)
            {
                return;
            }
        }

        if (!completed)
        {
            completed = true;
            StartCoroutine(ClosePanel());
        }
    }
    private void ResetMission()
    {
        completed = false;

        SymbolDrag[] existingSymbols = missionPanel.GetComponentsInChildren<SymbolDrag>(true);

        foreach (var symbol in existingSymbols)
        {
            Destroy(symbol.gameObject);
        }

        ShufflePositions(initialSymbolPositions);

        for (int i = 0; i < correctOrder.Length; i++)
        {
            Transform spawnPoint = initialSymbolPositions[i];

            GameObject newSymbol = Instantiate(symbolPrefab, initialSymbolContainer);


            RectTransform rt = newSymbol.GetComponent<RectTransform>();

            rt.anchoredPosition = spawnPoint.GetComponent<RectTransform>().anchoredPosition;

            SymbolDrag symbolDrag = newSymbol.GetComponent<SymbolDrag>();
            if (symbolDrag != null)
            {
                symbolDrag.canvas = rootCanvas;
                symbolDrag.miniGame = this;
            }

            newSymbol.GetComponent<Image>().sprite = correctOrder[i].sprite;
        }
    }
    private void ShufflePositions(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            (array[i], array[j]) = (array[j], array[i]);
        }
    }
    private IEnumerator ClosePanel()
    {
        MissionUIFeedback.Instance?.ShowMissionCompleted();

        yield return new WaitForSeconds(2f);
        missionPanel.SetActive(false);

        if (currentPlayer != null)
            currentPlayer.FreezePlayer(false);

        VideoGameManager.Instance.AddFireServerRpc(20f);

        //MissionSpawnManager.Instance.CompleteMissionServerRpc(missionObject);
        NetworkObjectReference missionRef = missionObject;
        MissionSpawnManager.Instance.CompleteMissionServerRpc(missionRef);
    }
}
