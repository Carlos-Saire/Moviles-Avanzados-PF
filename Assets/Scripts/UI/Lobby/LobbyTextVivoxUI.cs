using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class LobbyTextVivoxUI : MonoBehaviour
{
    [SerializeField] Transform panel;
    private void Reset()
    {
        gameObject.name = "LobbyTextVivoxUI";
    }
    private void OnEnable()
    {
        InputHandler.OnOpen += Open;
        InputHandler.onClouse += Clouse;
    }
    private void OnDisable()
    {
        InputHandler.OnOpen -= Open;
        InputHandler.onClouse -= Clouse;
    }
    private void Start()
    {
        CursorVisibility(false);
    }
    public void ButtonPress()
    {
        if (panel.gameObject.activeSelf)
        {
            Clouse();
        }
        else
        {
            Open();
        }
    }
    private void Open()
    {
        panel.gameObject.SetActive(true);
        CursorVisibility(true);
    }
    private void Clouse()
    {
        panel.gameObject.SetActive(false);
        CursorVisibility(false);
    }
    private void CursorVisibility(bool value)
    {
        if (value)
        {
            Cursor.visible = value;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = value;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
