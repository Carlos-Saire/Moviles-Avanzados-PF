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

    [Header("Camera Limit")]
    public Camera targetCamera;  

    Vector2 cursorPos;
    EventSystem eventSystem;
    List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();

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
            Canvas.willRenderCanvases += OnWillRenderCanvases;
    }

    void OnDisable()
    {
        if (autoRefreshRaycasters)
            Canvas.willRenderCanvases -= OnWillRenderCanvases;
    }

    void OnWillRenderCanvases()
    {
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
        if (targetCamera == null) return; 

        // CURSOR MOVEMENT
        Vector2 move = Gamepad.current.leftStick.ReadValue();
        if (move.magnitude > deadZone)
            cursorPos += move * cursorSpeed * Time.deltaTime;

        Rect pixelRect = targetCamera.pixelRect;
        cursorPos.x = Mathf.Clamp(cursorPos.x, pixelRect.xMin, pixelRect.xMax);
        cursorPos.y = Mathf.Clamp(cursorPos.y, pixelRect.yMin, pixelRect.yMax);

        currentPointerData.Reset();
        currentPointerData.position = cursorPos;
        currentPointerData.delta = Vector2.zero;
        currentPointerData.scrollDelta = Vector2.zero;
        currentPointerData.pointerId = -1;

        List<RaycastResult> uiResults = new List<RaycastResult>();
        eventSystem.RaycastAll(currentPointerData, uiResults);

        GameObject hovered = uiResults.Count > 0 ? uiResults[0].gameObject : null;

        if (hovered != lastHoveredObject)
        {
            if (lastHoveredObject != null)
                ExecuteEvents.Execute(lastHoveredObject, currentPointerData, ExecuteEvents.pointerExitHandler);

            if (hovered != null)
                ExecuteEvents.Execute(hovered, currentPointerData, ExecuteEvents.pointerEnterHandler);

            lastHoveredObject = hovered;
        }

        bool pressed = Gamepad.current.buttonSouth.wasPressedThisFrame;
        bool released = Gamepad.current.buttonSouth.wasReleasedThisFrame;

        if (pressed)
        {
            isPressed = true;

            currentPointerData.pressPosition = currentPointerData.position;
            currentPointerData.pointerPressRaycast = uiResults.Count > 0 ? uiResults[0] : new RaycastResult();

            if (hovered != null)
            {
                ExecuteEvents.ExecuteHierarchy(hovered, currentPointerData, ExecuteEvents.pointerDownHandler);
                GameObject newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(hovered);
                currentPointerData.pointerPress = newPressed;
                currentPointerData.rawPointerPress = hovered;
            }
            else
            {
                currentPointerData.pointerPress = null;
                currentPointerData.rawPointerPress = null;
            }
        }

        if (released && isPressed)
        {
            isPressed = false;

            GameObject pointerUpTarget = currentPointerData.rawPointerPress != null ? currentPointerData.rawPointerPress : hovered;

            if (pointerUpTarget != null)
            {
                ExecuteEvents.Execute(pointerUpTarget, currentPointerData, ExecuteEvents.pointerUpHandler);

                GameObject clickHandler = currentPointerData.pointerPress;
                if (clickHandler != null)
                    ExecuteEvents.Execute(clickHandler, currentPointerData, ExecuteEvents.pointerClickHandler);
            }

            currentPointerData.pointerPress = null;
            currentPointerData.rawPointerPress = null;
        }
    }

    void OnGUI()
    {
        if (forceHidden) return;
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

        if (active)
        {
            if (targetCamera != null)
            {
                cursorPos = targetCamera.pixelRect.center;
            }
        }
        else
        {
            lastHoveredObject = null;
            isPressed = false;
        }
    }
}
