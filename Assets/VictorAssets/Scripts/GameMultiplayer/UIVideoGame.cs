using TMPro;
using UnityEngine;
using SinglePlayer;
using System;
public class UIVideoGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fireText;
    [SerializeField] private TextMeshProUGUI timerText;
    //[SerializeField] private GameObject LosePanel;
    public static event Action<float> OnFireChanged;
    public static event Action<int,int> OnTimeChanged;
    private void Update()
    {
        //Debug.Log("UI Update");
        //Debug.Log(SinglePlayer.VideoGameManager.Instance+ "nose");
        if (SinglePlayer.VideoGameManager.Instance == null) return;

        float fire = SinglePlayer.VideoGameManager.Instance.GetFire();
        float time = SinglePlayer.VideoGameManager.Instance.GetTimer();
        OnFireChanged?.Invoke(fire);
        //fireText.text = Mathf.RoundToInt(fire) + "%";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
        OnTimeChanged?.Invoke(minutes,seconds);
    }
    public void ActiveLose()
    {
        //LosePanel.SetActive(true);
    }
}
