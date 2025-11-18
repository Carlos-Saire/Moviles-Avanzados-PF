using UnityEngine;
using Command;
using UnityEngine.UI;
using System.Collections;
public class InstallerLogin : MonoBehaviour
{
    [SerializeField] private CanvasGroup logo;
    [Header("Panels")]
    [SerializeField] private Transform intro;
    [SerializeField] private Transform panelConnected;
    [SerializeField] private Slider slider;
    [SerializeField] private float duration;

    [Header("AuthenticationManager")]
    [SerializeField] private AuthenticationManager authentication;
    private void Start()
    {
        Invoke("BeginAnimation", 1);
    }
    private void BeginAnimation()
    {
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(logo, 1, 1f));
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(logo, 0, 1f));
        CommandQueue.Instance.AddCommand(new SetActiveCommand(panelConnected.gameObject,true));
        CommandQueue.Instance.AddCommand(new SetActiveCommand(intro.gameObject, false));
        authentication.InitializeServices();
        CommandQueue.Instance.AddCommand(new SliderCommand(slider, duration));
    }
}
