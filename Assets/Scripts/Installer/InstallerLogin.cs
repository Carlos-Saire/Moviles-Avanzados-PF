using UnityEngine;
using Command;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class InstallerLogin : MonoBehaviour
{
    [SerializeField] private CanvasGroup logo;

    [SerializeField] private GameObject currentSelect;

    [Header("Panels")]
    [SerializeField] private Transform intro;
    [SerializeField] private Transform panelConnected;

    [Header("Slider")]
    [SerializeField] private Slider slider;
    [SerializeField] private float duration;

    [SerializeField] private Transform panelReiniciar;
    [SerializeField] private Transform panelSinglePlayer;

    [SerializeField] private Button reiniciar;

    [SerializeField] private CanvasGroup fade;

    [Header("UnityServicesManager")]
    [SerializeField] private UnityServicesManager unityServices;

    [Header("UnityServicesManager")]
    [SerializeField] private AuthenticationManager authenticationManager;
    private void OnEnable()
    {
        reiniciar?.onClick.AddListener(reiniciarPress);
        AuthenticationManager.OnSignIn += SignIn;
    }
    private void OnDisable()
    {
        reiniciar?.onClick.RemoveListener(reiniciarPress);
        AuthenticationManager.OnSignIn -= SignIn;
    }
    private void Start()
    {
        Invoke("BeginAnimation", 1);

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
#if UNITY_ANDROID
        RequestMicrophonePermission();
        ConfigScreen();
#endif
    }
    private void BeginAnimation()
    {
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(logo, 1, 1f));
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(logo, 0, 1f));
        CommandQueue.Instance.AddCommand(new SetActiveCommand(panelConnected.gameObject,true));
        CommandQueue.Instance.AddCommand(new SetActiveCommand(intro.gameObject, false));
#if !UNITY_WSA_10_0
        unityServices.InitializeServices();
#endif
        CommandQueue.Instance.AddCommand(new SliderCommand(slider, duration));
        CommandQueue.Instance.AddCommand(new GenericCommad(CheckAuthentication));
    }
    private void CheckAuthentication()
    {
        if (unityServices.AreServicesInitialized())
        {
            if (authenticationManager.CheckSession())
            {
                return;
            }
            CommandQueue.Instance.AddCommand(new LoadSceneCommand("Login"));
        }
        else
        {
#if UNITY_WSA_10_0
        panelSinglePlayer.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentSelect);
#else
            panelReiniciar.gameObject.SetActive(true);
#endif
        }

    }
    private void reiniciarPress()
    {
        CommandQueue.Instance.AddCommand(new CanvasFadeCommand(fade, 1, 0.5f));
        CommandQueue.Instance.AddCommand(new LoadSceneCommand("Intro"));
    }
    private async void SignIn()
    {
        await CloudSaveManager.Instance.LoadProfileAsync();
        CommandQueue.Instance.AddCommand(new LoadSceneCommand("Menu"));
    }
#if UNITY_ANDROID

    private void RequestMicrophonePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
    }
    private void ConfigScreen()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.AutoRotation;

        int w = Screen.currentResolution.width;
        int h = Screen.currentResolution.height;

        Screen.SetResolution((int)(w * 0.7f), (int)(h * 0.7f), true);
    }
#endif
}
