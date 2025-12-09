using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UniversalGamepadCursorV2 : MonoBehaviour
{
    private bool forceHidden = false;

    [Header("Cursor movement")]
    public float cursorSpeed = 900f;
    public float deadZone = 0.2f;

    [Header("Cursor GUI")]
    public Texture2D cursorTexture;
    public Vector2 cursorSize = new Vector2(32, 32);

    [Header("Behavior")]
    public bool debugLogs = false;
    public bool autoRefreshRaycasters = true;

    Vector2 cursorPos;
    EventSystem eventSystem;
    List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();

    // State for proper click simulation
    GameObject lastHoveredObject;
    bool isPressed = false;
    PointerEventData currentPointerData;

    void Awake()
    {
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem = go.GetComponent<EventSystem>();
        }

        RefreshRaycasters();
    }

    void OnEnable()
    {
        if (autoRefreshRaycasters)
            Canvas.willRenderCanvases += OnWillRenderCanvases; // refresh if canvases change
    }

    void OnDisable()
    {
        if (autoRefreshRaycasters)
            Canvas.willRenderCanvases -= OnWillRenderCanvases;
    }

    void OnWillRenderCanvases()
    {
        // refresca si hay canvases nuevos o removidos en ejecución
        RefreshRaycasters();
    }

    void RefreshRaycasters()
    {
        raycasters.Clear();
        raycasters.AddRange(FindObjectsOfType<GraphicRaycaster>());
        if (debugLogs) Debug.Log("[Cursor] Raycasters found: " + raycasters.Count);
    }

    void Start()
    {
        cursorPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        currentPointerData = new PointerEventData(eventSystem) { pointerId = -1 };

        EnableCursor(false);
    }

    void Update()
    {
        if (Gamepad.current == null) return;

        // Move cursor
        Vector2 move = Gamepad.current.leftStick.ReadValue();
        if (move.magnitude > deadZone)
            cursorPos += move * cursorSpeed * Time.deltaTime;

        cursorPos.x = Mathf.Clamp(cursorPos.x, 0, Screen.width);
        cursorPos.y = Mathf.Clamp(cursorPos.y, 0, Screen.height);

        // Prepare PointerEventData
        currentPointerData.Reset();
        currentPointerData.position = cursorPos;
        currentPointerData.delta = Vector2.zero;
        currentPointerData.scrollDelta = Vector2.zero;
        currentPointerData.pointerId = -1;

        // UI Raycast using EventSystem (works for all Canvas types)
        List<RaycastResult> uiResults = new List<RaycastResult>();
        eventSystem.RaycastAll(currentPointerData, uiResults);

        GameObject hovered = uiResults.Count > 0 ? uiResults[0].gameObject : null;

        // Pointer enter / exit
        if (hovered != lastHoveredObject)
        {
            if (lastHoveredObject != null)
            {
                ExecuteEvents.Execute(lastHoveredObject, currentPointerData, ExecuteEvents.pointerExitHandler);
                if (debugLogs) Debug.Log("[Cursor] pointerExit -> " + lastHoveredObject.name);
            }

            if (hovered != null)
            {
                ExecuteEvents.Execute(hovered, currentPointerData, ExecuteEvents.pointerEnterHandler);
                if (debugLogs) Debug.Log("[Cursor] pointerEnter -> " + hovered.name);
            }

            lastHoveredObject = hovered;
        }

        // Handle press / release (simulate mouse/touch properly)
        bool pressed = Gamepad.current.buttonSouth.wasPressedThisFrame;
        bool released = Gamepad.current.buttonSouth.wasReleasedThisFrame;

        if (pressed)
        {
            isPressed = true;

            // fill press-related fields
            currentPointerData.pressPosition = currentPointerData.position;
            currentPointerData.pointerPressRaycast = uiResults.Count > 0 ? uiResults[0] : new RaycastResult();

            // Execute pointerDown on the topmost object under cursor (hierarchy)
            if (hovered != null)
            {
                ExecuteEvents.ExecuteHierarchy(hovered, currentPointerData, ExecuteEvents.pointerDownHandler);
                // Set pointerPress so Button can detect it later
                GameObject newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(hovered);
                currentPointerData.pointerPress = newPressed;
                currentPointerData.rawPointerPress = hovered;
                if (debugLogs) Debug.Log("[Cursor] pointerDown -> " + hovered.name + "  pressHandler: " + (newPressed ? newPressed.name : "null"));
            }
            else
            {
                // No UI under cursor: still mark pressed to later send to world objects if needed
                currentPointerData.pointerPress = null;
                currentPointerData.rawPointerPress = null;
                if (debugLogs) Debug.Log("[Cursor] pressed with no UI under cursor");
            }
        }

        if (released && isPressed)
        {
            isPressed = false;

            // pointerUp on the object that received pointerDown (rawPointerPress) OR current hovered
            GameObject pointerUpTarget = currentPointerData.rawPointerPress != null ? currentPointerData.rawPointerPress : hovered;

            if (pointerUpTarget != null)
            {
                ExecuteEvents.Execute(pointerUpTarget, currentPointerData, ExecuteEvents.pointerUpHandler);
                if (debugLogs) Debug.Log("[Cursor] pointerUp -> " + pointerUpTarget.name);

                // If pointerPress handler exists and it's same as pointerUp target, do click
                GameObject clickHandler = currentPointerData.pointerPress;
                if (clickHandler != null)
                {
                    ExecuteEvents.Execute(clickHandler, currentPointerData, ExecuteEvents.pointerClickHandler);
                    if (debugLogs) Debug.Log("[Cursor] pointerClick -> " + clickHandler.name);
                }
            }

            // reset press fields
            currentPointerData.pointerPress = null;
            currentPointerData.rawPointerPress = null;
        }
    }

    // Draw cursor with OnGUI so it's always visible regardless of Canvas
    void OnGUI()
    {
        if (forceHidden) return;        // <---- BLOQUEA SI O SI

        if (!this.enabled) return;

        if (cursorTexture != null)
        {
            Rect rect = new Rect(
                cursorPos.x - cursorSize.x / 2f,
                Screen.height - cursorPos.y - cursorSize.y / 2f,
                cursorSize.x,
                cursorSize.y
            );
            GUI.DrawTexture(rect, cursorTexture);
        }
    }
    public void EnableCursor(bool active)
    {
        this.enabled = active;
        forceHidden = !active;

        if (!active)
        {
            lastHoveredObject = null;
            isPressed = false;
        }
    }
}
