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
    [SerializeField] private Transform UImobile;

#if UNITY_ANDROID 
    private float _width;
    private float _currentPrees;
    private void Start()
    {
        _width = Screen.width/2;
        UImobile.gameObject.SetActive(true);
    }
#endif
    public void InputMove(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void InputPress(InputAction.CallbackContext context)
    {
#if UNITY_ANDROID

        if (context.performed)
        {
            _currentPrees = Touchscreen.current.primaryTouch.position.ReadValue().x;
        }
#endif
    }
    public void InputLook(InputAction.CallbackContext context)
    {
#if UNITY_ANDROID
        if (_width < _currentPrees)
        {
            OnLook?.Invoke(context.ReadValue<Vector2>());
        }
#else
        OnLook?.Invoke(context.ReadValue<Vector2>());
#endif
    }
}
