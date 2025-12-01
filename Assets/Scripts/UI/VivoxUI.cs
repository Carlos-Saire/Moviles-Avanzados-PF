using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Command;
public class VivoxUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private Transform panelText;

    [Header("ChannelText")]
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private TMP_InputField inputFieldText;

    [Header("Microphone")]
    [SerializeField] private Button micButton;
    [SerializeField] private Image image;
    private bool isMuted;

    [Header("Microphone")]

    [Header("VivoxManager")]
    [SerializeField] private VivoxManager vivoxManager;
    private void Reset()
    {
        gameObject.name = "VivoxUI";
    }
    private void OnEnable()
    {
        inputFieldText?.onSubmit.AddListener(SubmitText);
        VivoxManager.OnMessageReceived += CreateText;
        micButton?.onClick.AddListener(OnMicButtonPressed);
    }
    private void OnDisable()
    {
        inputFieldText?.onSubmit.RemoveListener(SubmitText);
        VivoxManager.OnMessageReceived -= CreateText;
        micButton?.onClick.RemoveListener(OnMicButtonPressed);

    }
    private void Start()
    {
        isMuted = !vivoxManager.IsMicrophoneOn();
        OnMicButtonPressed();
    }
    private void SubmitText(string arg0)
    {
        vivoxManager.SendMessageToChannel(arg0);
    }
    private void CreateText(string arg0)
    {
        RectTransform newLobby = Instantiate(prefab);
        newLobby.SetParent(content);
        newLobby.localScale = Vector3.one;
        newLobby.GetComponent<TextVivoxInfo>().UpdateInformation(arg0);
    }
    private void OnMicButtonPressed()
    {
        if (isMuted)
        {
            CommandQueue.Instance.AddCommand(new TurnMicOnCommand(vivoxManager));
            image.color = new Color(1, 1, 1);
            isMuted = false;
        }
        else
        {
            CommandQueue.Instance.AddCommand(new TurnMicOffCommand(vivoxManager));
            image.color = new Color(0, 0, 0);
            isMuted = true;
        }
    }
}
