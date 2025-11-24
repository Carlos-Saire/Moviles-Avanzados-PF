using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineHoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Opciones del borde")]
    public Color borderColor = Color.white;
    public float borderThickness = 3f;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.effectColor = borderColor;
        outline.effectDistance = new Vector2(borderThickness, borderThickness);
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
}
