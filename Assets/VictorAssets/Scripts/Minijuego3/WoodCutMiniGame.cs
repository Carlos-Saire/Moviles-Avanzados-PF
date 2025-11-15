using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WoodCutMiniGame : MonoBehaviour
{
    [Header("Panel de la misión")]
    public GameObject missionPanel;    

    [Header("Tronco")]
    public Image logImage;
    public Sprite logCutSprite;

    [Header("Lineas a cortar")]
    public Button[] cutLines;

    private int linesLeft;
    private bool completed = false;

    private void OnEnable()
    {
        ResetMission();
    }

    private void ResetMission()
    {
        linesLeft = cutLines.Length;
        completed = false;

        for (int i = 0; i < cutLines.Length; i++)
        {
            cutLines[i].gameObject.SetActive(true);

            int index = i;
            cutLines[i].onClick.RemoveAllListeners();
            cutLines[i].onClick.AddListener(() => CutLine(index));
        }
    }

    private void CutLine(int index)
    {
        cutLines[index].gameObject.SetActive(false);
        linesLeft--;

        if (linesLeft <= 0 && !completed)
        {
            completed = true;
            logImage.sprite = logCutSprite;

            StartCoroutine(CloseMissionPanel());
        }
    }

    private IEnumerator CloseMissionPanel()
    {
        yield return new WaitForSeconds(3f);
        missionPanel.SetActive(false);
    }
}
