using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMove;
    public event Action<Vector2> OnMoveSinglePLayer;

    public static event Action<Vector2> OnLook;
    public event Action<Vector2> OnLookSinglePLayer;

    public static event Action OnAttack;
    public static event Action OnInteract;
    [SerializeField] private Transform UImobile;

    public void InputMove(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
        OnMoveSinglePLayer?.Invoke(context.ReadValue<Vector2>());
        Debug.Log(context.ReadValue<Vector2>());
    }
    public void InputLook(InputAction.CallbackContext context)
    {
        OnLook?.Invoke(context.ReadValue<Vector2>());
    }
    public void InputAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttack?.Invoke();
        }
    }
    public void InputInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteract?.Invoke();
        }
    }
}
