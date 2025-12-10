using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text fireText;

    [Header("Panels")]
    [SerializeField] private GameObject fail;
    [SerializeField] private GameObject Win;
    private void OnEnable()
    {
        UIVideoGame.OnFireChanged += UpdateFireUI;
        UIVideoGame.OnTimeChanged += UpdateTimerUI;
    }
    private void OnDisable()
    {
        UIVideoGame.OnFireChanged -= UpdateFireUI;
        UIVideoGame.OnTimeChanged -= UpdateTimerUI;
    }
    private void UpdateTimerUI(int arg1, int arg2)
    {
        timerText.text = $"{arg1:00}:{arg2:00}";
    }

    private void UpdateFireUI(float obj)
    {
        fireText.text=Mathf.RoundToInt(obj) + "%";
    }
    public void Fail()
    {
        fail.gameObject.SetActive(true);
    }
  
}
