using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamepadChecker : MonoBehaviour
{
    [SerializeField] private Button P1;
    [SerializeField] private Button P2;
    [SerializeField] private Button P3;
    [SerializeField] private Button P4;

    private Button[] botones;

    private void Reset()
    {
        gameObject.name = "GamepadChecker";
    }

    private void Awake()
    {
        botones = new Button[] { P1, P2, P3, P4 };

        InputSystem.onDeviceChange += OnDeviceChange;
        ActualizarBotones();
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
    private void Start()
    {
        //for (int i = Gamepad.all.Count; i < 3; i++) {  InputSystem.AddDevice<Gamepad>(); }
    }
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
            ActualizarBotones();
    }

    private void ActualizarBotones()
    {
        int cantidad = Gamepad.all.Count;

        for (int i = 0; i < botones.Length; i++)
        {
            bool activo = (i < cantidad);

            botones[i].interactable = activo;

            Navigation nav = botones[i].navigation;
            nav.mode = activo ? Navigation.Mode.Explicit : Navigation.Mode.None;
            botones[i].navigation = nav;
        }

        for (int i = 0; i < botones.Length; i++)
        {
            if (!botones[i].interactable)
                continue;

            Navigation nav = botones[i].navigation;

            if (i + 1 < botones.Length && botones[i + 1].interactable)
                nav.selectOnRight = botones[i + 1];
            else
                nav.selectOnRight = null;

            if (i - 1 >= 0 && botones[i - 1].interactable)
                nav.selectOnLeft = botones[i - 1];
            else
                nav.selectOnLeft = null;

            botones[i].navigation = nav;
        }
    }
    public void SeleccionarJugadores(int cantidad)
    {
        PlayerPrefs.SetInt("JugadoresSeleccionados", cantidad);
    }
}
