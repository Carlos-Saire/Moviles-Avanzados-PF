using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InputDebugOnScreen : MonoBehaviour
{
    [Header("Actions")]
    public InputActionReference submitAction;
    public InputActionReference navigateAction;

    private List<string> logs = new List<string>();
    private Vector2 scroll;

    private void OnEnable()
    {
        if (submitAction != null)
            submitAction.action.performed += OnSubmit;

        if (navigateAction != null)
            navigateAction.action.performed += OnNavigate;
    }

    private void OnDisable()
    {
        if (submitAction != null)
            submitAction.action.performed -= OnSubmit;

        if (navigateAction != null)
            navigateAction.action.performed -= OnNavigate;
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        AddLog($"SUBMIT  Device: {ctx.control.device.displayName}, Control: {ctx.control.name}");
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        AddLog($"NAVIGATE  Device: {ctx.control.device.displayName}, Control: {ctx.control.name}");
    }

    private void AddLog(string msg)
    {
        logs.Add(msg + "   [" + System.DateTime.Now.ToLongTimeString() + "]");
        if (logs.Count > 20)
            logs.RemoveAt(0); // mantener limpio
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 18;
        style.normal.textColor = Color.green;

        GUILayout.BeginArea(new Rect(10, 10, 700, 600));
        GUILayout.Label(" DEBUG INPUT SYSTEM", style);

        scroll = GUILayout.BeginScrollView(scroll, GUILayout.Width(700), GUILayout.Height(400));
        foreach (var log in logs)
            GUILayout.Label(log, style);
        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }
}
