using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Command;
public class WelcomeUI : MonoBehaviour
{
    [Header("Dropdown")]
    [SerializeField]public TMP_Dropdown dayDropdown;
    [SerializeField]public TMP_Dropdown monthDropdown;
    [SerializeField]public TMP_Dropdown yearDropdown;

    private string description;

    [SerializeField] private PlayerInfoSO playerInfoSO;
    private void Reset()
    {
        gameObject.name = "WelcomeUI";
    }
    private void Start()
    {
        SetupDay();
        SetupMonth();
        SetupYear();
    }
    private void SetupDay()
    {
        dayDropdown.ClearOptions();
        List<string> days = new List<string>();
        for (int i = 1; i <= 31; i++)
            days.Add(i.ToString());

        dayDropdown.AddOptions(days);
    }

    private void SetupMonth()
    {
        monthDropdown.ClearOptions();
        List<string> months = new List<string>();
        for (int i = 1; i <= 12; i++)
            months.Add(i.ToString());

        monthDropdown.AddOptions(months);
    }

    private void SetupYear()
    {
        yearDropdown.ClearOptions();
        List<string> years = new List<string>();

        int currentYear = DateTime.Now.Year;
        for (int y = currentYear - 100; y <= currentYear; y++)
            years.Add(y.ToString());

        years.Reverse();
        yearDropdown.AddOptions(years);
    }

    public string GetDate()
    {
        string d = dayDropdown.options[dayDropdown.value].text;
        string m = monthDropdown.options[monthDropdown.value].text;
        string y = yearDropdown.options[yearDropdown.value].text;

        return $"{y}-{m}-{d}";
    }
    public void SetDescription(string value)
    {
        description = value;
    }
    public void ButtonConfimr()
    {
        if (ProfileImageSelector.SelectedImageIndex != null)
        {
            playerInfoSO.PlayerIndexProfile =(int)ProfileImageSelector.SelectedImageIndex;
            playerInfoSO.Playerbirthday = GetDate();
            playerInfoSO.PlayerDescription = description;
            CommandQueue.Instance.AddCommand(new LoadSceneCommand("Menu"));
            Debug.Log("Data Save Plyer Info SO");
        }
        else
        {
            Debug.Log("Falta data");
        }
    }

}
