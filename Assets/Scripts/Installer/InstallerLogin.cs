using UnityEngine;
using Command;
using Unity.Services.Core;
public class InstallerLogin : MonoBehaviour
{
    [SerializeField] private CanvasGroup logo;

    private void Start()
    {
        Invoke("BeginAnimation", 1);
    }
    private void BeginAnimation()
    {
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(logo, 1, 1f));
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(logo, 0, 1f));
    }

}
