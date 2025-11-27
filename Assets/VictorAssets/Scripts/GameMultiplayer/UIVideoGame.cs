using TMPro;
using UnityEngine;

public class UIVideoGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fireText;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Update()
    {
        if (VideoGameManager.Instance == null) return;

        float fire = VideoGameManager.Instance.GetFire();
        float time = VideoGameManager.Instance.GetTimer();

        fireText.text = Mathf.RoundToInt(fire) + "%";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
