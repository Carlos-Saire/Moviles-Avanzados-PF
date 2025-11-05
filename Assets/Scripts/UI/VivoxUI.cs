using TMPro;
using UnityEngine;

public class VivoxUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private Transform panelText;

    [Header("ChannelText")]
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private TMP_InputField inputFieldText;

    private void Reset()
    {
        gameObject.name = "VivoxUI";
    }
    private void OnEnable()
    {
        inputFieldText?.onSubmit.AddListener(SubmitText);
        VivoxManager.OnMessageReceived += CreateText;
    }
    private void OnDisable()
    {
        inputFieldText?.onSubmit.RemoveListener(SubmitText);
        VivoxManager.OnMessageReceived -= CreateText;
    }
    private void SubmitText(string arg0)
    {
        VivoxManager.instance.SendMessageToChannel(arg0);
    }
    private void CreateText(string arg0)
    {
        RectTransform newLobby = Instantiate(prefab);
        newLobby.SetParent(content);
        newLobby.localScale = Vector3.one;
        newLobby.GetComponent<TextVivoxInformation>().UpdateInformation(arg0);
    }

}
