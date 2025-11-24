using TMPro;
using UnityEngine;
public class JoinCodeUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text errorText;
    private string code;
    private void Reset()
    {
        gameObject.name = "JoinCodeUI";
    }
    private void OnEnable()
    {
        inputField.onValueChanged.AddListener(ValueChanged);
        inputField.onSubmit.AddListener(OnSubmit);
    }
    private void OnDisable()
    {
        inputField.onValueChanged.AddListener(ValueChanged);
        inputField.onSubmit.RemoveListener(OnSubmit);
    }
    private void ValueChanged(string arg0)
    {
        string noSpaces = arg0.Replace(" ", "");
        if (noSpaces != arg0)
        {
            inputField.text = noSpaces;
        }
        code = noSpaces;
    }
    private void OnSubmit(string arg0)
    {
        JoinCodePress();
    }
    public void JoinCodePress()
    {
        JoinCode(code);
    }
    private void JoinCode(string Code)
    {
        LobbyManager.instance.JoinLobbyByCode(Code);
    }
}
