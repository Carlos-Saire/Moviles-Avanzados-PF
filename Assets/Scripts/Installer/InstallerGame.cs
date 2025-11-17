using UnityEngine;
using Command;
public class InstallerGame : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasFade;
    private void Reset()
    {
        gameObject.name = "InstallerGame";
    }
    private void Start()
    {
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(canvasFade,0,0.5f));
    }
}
