using TMPro;
using UnityEngine;
using SinglePlayer;
public class UIVideoGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fireText;
    [SerializeField] private TextMeshProUGUI timerText;
    //[SerializeField] private GameObject LosePanel;
    private void Update()
    {
        if (SinglePlayer.VideoGameManager.Instance == null) return;

        float fire = SinglePlayer.VideoGameManager.Instance.GetFire();
        float time = SinglePlayer.VideoGameManager.Instance.GetTimer();

        fireText.text = Mathf.RoundToInt(fire) + "%";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    public void ActiveLose()
    {
        //LosePanel.SetActive(true);
    }
}
