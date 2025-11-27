using UnityEngine;
using TMPro;
using System.Collections;

public class MissionUIFeedback : MonoBehaviour
{
    public static MissionUIFeedback Instance;

    [Header("Texto de misión completada")]
    public TMP_Text missionCompletedText;

    private void Awake()
    {
        Instance = this;

        if (missionCompletedText != null)
            missionCompletedText.gameObject.SetActive(false);
    }

    public void ShowMissionCompleted()
    {
        StartCoroutine(ShowMessageRoutine());
    }

    private IEnumerator ShowMessageRoutine()
    {
        missionCompletedText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        missionCompletedText.gameObject.SetActive(false);
    }
}
