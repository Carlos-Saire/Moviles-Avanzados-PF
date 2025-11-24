using TMPro;
using UnityEngine;

public class EditNameUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private Transform editNamePanel;

    [Header("InputField")]
    [SerializeField] private TMP_InputField editNameInputField;
    private string newName;

    private void OnEnable()
    {
        AuthenticationManager.OnNameUpdated += Cancel;

        editNameInputField.onValueChanged.AddListener(ValueChangedPlayerName);
        editNameInputField.onSubmit.AddListener(OnSubmitPlayerName);
    }
    private void OnDisable()
    {
        AuthenticationManager.OnNameUpdated -= Cancel;

        editNameInputField.onValueChanged.RemoveListener(ValueChangedPlayerName);
        editNameInputField.onSubmit.RemoveListener(OnSubmitPlayerName);
    }
    private void ValueChangedPlayerName(string arg0)
    {
        string noSpaces = arg0.Replace(" ", "");
        if (noSpaces != arg0)
        {
            editNameInputField.text = noSpaces;
        }
        newName = noSpaces;
    }
    private void OnSubmitPlayerName(string arg0)
    {
        if (arg0 != "")
        {
            AuthenticationManager.Instance.EditNameAsync(arg0);
            editNameInputField.text = "";
        }
    }
    public void ConfirmsName()
    {
        AuthenticationManager.Instance.EditNameAsync(newName);
        editNameInputField.text = "";
    }
    public void Cancel()
    {
        editNamePanel.gameObject.SetActive(false);
    }
}
