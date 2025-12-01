using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInputHandler_Directions : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float inputCooldown = 0.20f; // evita spam
    private bool inputLocked = false;

    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private EventSystem eventSystem;

    private InputAction navigate;
    private InputAction submit;
    private InputAction cancel;

    private void Awake()
    {
        if (eventSystem == null)
            eventSystem = EventSystem.current;

        var ui = inputActions.FindActionMap("UI");

        navigate = ui.FindAction("Navigate");
        submit = ui.FindAction("Submit");
        cancel = ui.FindAction("Cancel");
    }

    private void OnEnable()
    {
        navigate.Enable();
        submit.Enable();
        cancel.Enable();

        navigate.performed += OnNavigate;
        submit.performed += OnSubmit;
        cancel.performed += OnCancel;
    }

    private void OnDisable()
    {
        navigate.performed -= OnNavigate;
        submit.performed -= OnSubmit;
        cancel.performed -= OnCancel;

        navigate.Disable();
        submit.Disable();
        cancel.Disable();
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (inputLocked) return;

        Vector2 dir = ctx.ReadValue<Vector2>();
        string direction = "";

        if (dir.y > 0.5f) direction = "UP";
        else if (dir.y < -0.5f) direction = "DOWN";
        else if (dir.x < -0.5f) direction = "LEFT";
        else if (dir.x > 0.5f) direction = "RIGHT";
        else return; // no dirección

        Debug.Log("<color=yellow>[UI] Direction:</color> " + direction);

        StartCoroutine(Cooldown());
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (inputLocked) return;
        Debug.Log("<color=green>[UI] SUBMIT</color>");
        StartCoroutine(Cooldown());
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        if (inputLocked) return;
        Debug.Log("<color=red>[UI] CANCEL</color>");
        StartCoroutine(Cooldown());
    }

    private System.Collections.IEnumerator Cooldown()
    {
        inputLocked = true;
        yield return new WaitForSeconds(inputCooldown);
        inputLocked = false;
    }


}

