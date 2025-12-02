using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMove;
    public event Action<Vector2> OnMoveSinglePLayer;

    public static event Action<Vector2> OnLook;
    public event Action<Vector2> OnLookSinglePLayer;

    public static event Action OnAttack;
    public static event Action OnInteract;
    public static event Action OnOpen;
    public static event Action onClouse;
    [SerializeField] private Transform UImobile;

    static public bool IsMove = true;
#if UNITY_ANDROID|| UNITY_IOS
    private float _width;
    private float _currentPrees;

    private bool lookTouchActive = false;
    private int lookFingerId = -1;
    private Vector2 previousPos;


    private void Start()
    {
        _width = Screen.width / 2;
        UImobile.gameObject.SetActive(true);
    }
    private void Update()
    {

        if (!IsMove) return;

        if (Touchscreen.current == null) return;

        // Leemos TODOS los dedos
        var touches = Touchscreen.current.touches;

        foreach (var t in touches)
        {
            if (!t.press.isPressed) continue;

            int fingerId = t.touchId.ReadValue();
            Vector2 pos = t.position.ReadValue();

            if (t.press.wasPressedThisFrame)
            {
                if (pos.x >= _width)
                {
                    lookTouchActive = true;
                    lookFingerId = fingerId;
                    previousPos = pos;
                    Debug.Log("LOOK ACTIVADO dedo " + fingerId);
                }
            }

            if (lookTouchActive && fingerId == lookFingerId)
            {
                Vector2 delta = pos - previousPos;
                previousPos = pos;

                OnLook?.Invoke(delta);
                Debug.Log("DELTA: " + delta);
            }
        }

        foreach (var t in touches)
        {
            if (t.press.wasReleasedThisFrame)
            {
                int fingerId = t.touchId.ReadValue();

                if (lookTouchActive && fingerId == lookFingerId)
                {
                    lookTouchActive = false;
                    lookFingerId = -1;

                    OnLook?.Invoke(Vector2.zero);
                    Debug.Log("LOOK DESACTIVADO");
                }
            }
        }
    }
#endif
    public void InputMove(InputAction.CallbackContext context)
    {
        if (!IsMove) return;

        OnMove?.Invoke(context.ReadValue<Vector2>());
        OnMoveSinglePLayer?.Invoke(context.ReadValue<Vector2>());
        //Debug.Log("Move :" + context.ReadValue<Vector2>());
    }
    public void InputLook(InputAction.CallbackContext context)
    {
        if (!IsMove) return;

#if UNITY_ANDROID
        if (_width < _currentPrees)
        {
            OnLook?.Invoke(context.ReadValue<Vector2>());
        }
#else
        OnLook?.Invoke(context.ReadValue<Vector2>());
#endif
    }
    public void InputPress(InputAction.CallbackContext context)
    {
        if (!IsMove) return;

#if UNITY_ANDROID

        if (context.performed)
        {
            _currentPrees = Touchscreen.current.primaryTouch.position.ReadValue().x;
        }
#endif
    }
    public void InputAttack(InputAction.CallbackContext context)
    {
        if (!IsMove) return;

        if (context.performed)
        {
            OnAttack?.Invoke();
        }
    }
    public void InputInteract(InputAction.CallbackContext context)
    {
        if (!IsMove) return;

        if (context.started)
        {
            OnInteract?.Invoke();
        }
    }
    public void OpenInput(InputAction.CallbackContext context)
    {

        Debug.Log("Open");
        if (context.performed)
        {
            IsMove = false;
            OnOpen?.Invoke();
        }
    }
    public void ClouseInput(InputAction.CallbackContext context)
    {
        Debug.Log("Clouse");
        if (context.performed)
        {
            IsMove = true;
            onClouse?.Invoke();
        }
    }
}
