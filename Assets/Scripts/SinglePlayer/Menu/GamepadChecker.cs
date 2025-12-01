using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class GamepadChecker : MonoBehaviour
{
    [SerializeField] private Button P1;
    [SerializeField] private Button P2;
    [SerializeField] private Button P3;
    [SerializeField] private Button P4;

    private Button[] botones;
    private InputSystemUIInputModule uiInputModule; // Referencia al módulo de UI
    private void Reset()
    {
        gameObject.name = "GamepadChecker";
    }

    private void Awake()
    {
        botones = new Button[] { P1, P2, P3, P4 };
        uiInputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        if (uiInputModule == null)
        {
            Debug.LogError("InputSystemUIInputModule no encontrado.");
            return;
        }

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

        if (cantidad > 0)
        {
            // El Mando que se conectó primero siempre será Gamepad.all[0]
            Gamepad primerGamepad = Gamepad.all[0];

            // ⭐ ESTA ES LA ÚNICA LÍNEA CRUCIAL NECESARIA
            // Le dice al Action Asset de la UI que SÓLO escuche las entradas de este dispositivo.
            uiInputModule.actionsAsset.devices = new InputDevice[] { primerGamepad };

            // Asegúrate de que las acciones estén habilitadas (si no lo están ya)
            uiInputModule.actionsAsset.Enable();

            Debug.Log($"<color=green>UI Control Asignado a:</color> {primerGamepad.displayName}.");
        }
        else
        {
            // Si no hay gamepads, limpiamos la asignación para que otros dispositivos (ratón/teclado)
            // o el sistema por defecto puedan tomar el control.
            uiInputModule.actionsAsset.devices = new InputDevice[] { };
            Debug.Log("<color=yellow>UI Control:</color> Gamepads desconectados.");
        }

    
        // Primera parte: Desactivar no interactivos y establecer modo de navegación
        for (int i = 0; i < botones.Length; i++)
        {
            bool activo = (i < cantidad);
            botones[i].interactable = activo;

            Navigation nav = botones[i].navigation;
            // ⭐ Si no está activo, la navegación es NONE (no se puede seleccionar)
            nav.mode = activo ? Navigation.Mode.Explicit : Navigation.Mode.None;
            botones[i].navigation = nav;
        }

        // Segunda parte: Conectar solo los botones interactivos
        for (int i = 0; i < botones.Length; i++)
        {
            if (!botones[i].interactable)
                continue; // Ignorar botones no interactivos

            Navigation nav = botones[i].navigation;

            // Conectar a la DERECHA al siguiente botón interactivo (si existe)
            if (i + 1 < botones.Length && botones[i + 1].interactable)
                nav.selectOnRight = botones[i + 1];
            else
                nav.selectOnRight = null; // No hay siguiente

            // Conectar a la IZQUIERDA al botón interactivo anterior (si existe)
            if (i - 1 >= 0 && botones[i - 1].interactable)
                nav.selectOnLeft = botones[i - 1];
            else
                nav.selectOnLeft = null; // No hay anterior

            botones[i].navigation = nav;
        }
    }
    public void SeleccionarJugadores(int cantidad)
    {
        PlayerPrefs.SetInt("JugadoresSeleccionados", cantidad);
    }

}

public static class UISelector
{
    public static void SeleccionarInicial(Button btn)
    {
        // Paso 1: Limpia la selección previa (del Panel A).
        EventSystem.current.SetSelectedGameObject(null);

        // Paso 2: Establece la selección en el nuevo botón (en el Panel B).
        EventSystem.current.SetSelectedGameObject(btn.gameObject);
    }

}