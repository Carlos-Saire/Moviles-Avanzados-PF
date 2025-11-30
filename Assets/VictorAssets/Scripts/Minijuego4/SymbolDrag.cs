using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SymbolDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas;
    public SymbolOrderMiniGame miniGame;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 originalAnchoredPos;
    private Transform originalParent;

    private bool wasInsideSlot = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalParent = transform.parent;
        originalAnchoredPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        wasInsideSlot = transform.parent.GetComponent<SlotDropArea>() != null;

        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPos);

        rectTransform.anchoredPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        GameObject objBelow = eventData.pointerCurrentRaycast.gameObject;

        if (objBelow == null || objBelow.GetComponent<SlotDropArea>() == null)
        {
            ResetToOrigin();
            return;
        }

        SlotDropArea slot = objBelow.GetComponent<SlotDropArea>();

        if (slot.transform.childCount > 0)
        {
            ResetToOrigin();
            return;
        }

        transform.SetParent(slot.transform);
        rectTransform.anchoredPosition = Vector2.zero;

        miniGame.CheckOrder();
    }

    public void ResetToOrigin()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalAnchoredPos;
    }
}
