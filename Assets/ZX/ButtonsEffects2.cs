using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsEffects2 : MonoBehaviour, IPointerEnterHandler
{
  
    [SerializeField] private RectTransform highlightSprite;
    [Header("movimiento")]
    [SerializeField] private float highlightMoveDuration = 0.15f;
    [SerializeField] private Ease moveEase = Ease.OutQuad; 

    private RectTransform buttonRectTransform; 
    [Header("posición del sprite")]
    [SerializeField] private float yOffsetBelowButton = 30f;
    private void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        if (highlightSprite == null)
        {
            Debug.LogError("Falta" + gameObject.name);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightSprite != null)
        {
            highlightSprite.gameObject.SetActive(true);
            highlightSprite.DOAnchorPos(buttonRectTransform.anchoredPosition, highlightMoveDuration)
                .SetEase(moveEase);
        }
    }
}
