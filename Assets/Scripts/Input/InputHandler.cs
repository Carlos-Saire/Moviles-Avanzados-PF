using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMove;
    public void InputMove(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }
}
