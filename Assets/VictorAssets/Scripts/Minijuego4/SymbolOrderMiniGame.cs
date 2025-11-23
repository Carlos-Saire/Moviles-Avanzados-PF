using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SymbolOrderMiniGame : MonoBehaviour
{
    [Header("Panel de la misión")]
    public GameObject missionPanel;

    [Header("Slots correctos (arriba)")]
    public Image[] correctOrder;   

    [Header("Slots del jugador (abajo)")]
    public Transform[] playerSlots; 

    private bool completed = false;

    private PlayerController currentPlayer;

    public void SetPlayer(PlayerController player)
    {
        currentPlayer = player;
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

    private IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(2.5f);
        missionPanel.SetActive(false);

        if (currentPlayer != null)
            currentPlayer.FreezePlayer(false);
    }
}
