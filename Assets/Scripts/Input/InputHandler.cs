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
    [SerializeField] private Transform UImobile;


    //#if UNITY_ANDROID 
    //    private float _width;
    //    private float _currentPrees;
    //    private void Start()
    //    {
    //        _width = Screen.width/2;
    //        UImobile.gameObject.SetActive(true);
    //    }
    //#endif
    public void InputMove(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
        OnMoveSinglePLayer?.Invoke(context.ReadValue<Vector2>());
        Debug.Log(context.ReadValue<Vector2>());
    }
    public void InputLook(InputAction.CallbackContext context)
    {
//#if UNITY_ANDROID
//        if (_width < _currentPrees)
//        {
//            OnLook?.Invoke(context.ReadValue<Vector2>());
//        }
//#else
        OnLook?.Invoke(context.ReadValue<Vector2>());
        OnLookSinglePLayer?.Invoke(context.ReadValue<Vector2>());
        Debug.Log(context.ReadValue<Vector2>());

        //#endif
    }
    public void InputAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttack?.Invoke();
        }
    }
    //public void InputPress(InputAction.CallbackContext context)
    //{
        
    //}
}
