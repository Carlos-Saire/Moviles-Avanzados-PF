using System;
using Terresquall;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMove;
    public static event Action<Vector2> OnLook;
    [SerializeField] private VirtualJoystick virtualJoystick;
    public void InputMove(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }
    public void InputLook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 touchPos = context.ReadValue<Vector2>();
            float halfWidth = Screen.width / 2f;

            if (touchPos.x > halfWidth)
            {
                Debug.Log(" Toque en la parte DERECHA (landscape)");
            }
            else
            {
                Debug.Log("Toque en la parte IZQUIERDA (landscape)");
            }
        }

        if (context.canceled)
        {
            Debug.Log(" Toque finalizado");
        }
        OnLook?.Invoke(context.ReadValue<Vector2>());

    }
}
