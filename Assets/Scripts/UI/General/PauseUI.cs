using UnityEngine;
using Command;
using System.Threading.Tasks;
using UnityEngine.UI;
public class PauseUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private Transform panel;
    [SerializeField] private Transform panelText;

    [Header("Buttons")]
    [SerializeField] private Button buttonExit;

    private void Reset()
    {
        gameObject.name = "PauseUI";
    }
    private void OnEnable()
    {
        InputHandler.onClouse += PausePress;
        buttonExit?.onClick.AddListener(ExitPress);
    }
    private void OnDisable()
    {
        buttonExit?.onClick.RemoveListener(ExitPress);
        InputHandler.onClouse -= PausePress;
    }
    private void PausePress()
    {
        if (!panelText.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(true);
            CursorVisibility(true);
            InputHandler.IsMove = false;
        }
    }
    public void ReanudarPress()
    {
        panel.gameObject.SetActive(false);
        CursorVisibility(false);
        InputHandler.IsMove = true;
    }
    public async void ExitPress()
    {
        await LobbyManager.instance.RemovePlayerAsync();
        CommandQueue.Instance.AddCommand(new LoadSceneCommand("Menu"));
    }
    private void CursorVisibility(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
