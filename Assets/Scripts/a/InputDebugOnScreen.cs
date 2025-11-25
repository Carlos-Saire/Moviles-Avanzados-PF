using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class InputDebugOnScreen : MonoBehaviour
{
    [Header("Actions")]
    public InputActionReference submitAction;
    public InputActionReference navigateAction;

    private List<string> logs = new List<string>();
    private Vector2 scroll;
    //deteccion
    private bool canReadInput = true;
    public float inputDelay = 0.25f;
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
        if (!canReadInput) return;
        StartCoroutine(InputCooldown());

        LogInput($"SUBMIT  Device: {ctx.control.device.displayName}, Button: {ctx.control.name}");
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (!canReadInput) return;
        StartCoroutine(InputCooldown());

        Vector2 nav = ctx.ReadValue<Vector2>();

        // Limpieza  
        if (Mathf.Abs(nav.x) < 0.5f) nav.x = 0;
        if (Mathf.Abs(nav.y) < 0.5f) nav.y = 0;

        if (Mathf.Abs(nav.x) > Mathf.Abs(nav.y))
            nav.y = 0;
        else
            nav.x = 0;

        LogInput($"NAVIGATE {nav} Device: {ctx.control.device.displayName}");
    }

    
    private IEnumerator InputCooldown()
    {
        canReadInput = false;
        yield return new WaitForSeconds(inputDelay);
        canReadInput = true;
    }

    private void LogInput(string msg)
    {
        string finalMsg = $"{msg}  [{System.DateTime.Now.ToLongTimeString()}]";

        logs.Add(finalMsg);
        if (logs.Count > 20)
            logs.RemoveAt(0);

        Debug.Log(finalMsg);
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
