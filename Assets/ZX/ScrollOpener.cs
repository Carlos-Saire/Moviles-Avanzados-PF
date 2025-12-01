using UnityEngine;
using DG.Tweening;
using System.Collections.Generic; 
using System.Collections; 
public class ScrollOpener : MonoBehaviour
{
    [Header("UI")]
    public RectTransform leftPost;
    public RectTransform rightPost;
    public RectTransform contentPanel;
    public List<RectTransform> UI_Elements_To_Show;

    [Header("Animacion")]
    public float initialDelaySeconds = 1.0f; 
    public float openDuration = 0.5f;

    [Header("Posiciones de los postes")]
    
    public float finalY_LeftPost = 200f;  
    public float finalY_RightPost = 200f; 

    [Header("Botones")]
    public float buttonStaggerDelay = 0.05f;
    public float buttonInitialY_Offset = 50f; 

    private List<Vector2> originalButtonPositions = new List<Vector2>();
    public RectTransform highlightSprite;
    private void Start()
    {

        foreach (var element in UI_Elements_To_Show)
        {
            originalButtonPositions.Add(element.anchoredPosition);
            Vector2 initialPos = new Vector2(
                element.anchoredPosition.x,
                element.anchoredPosition.y - buttonInitialY_Offset
            );
            element.anchoredPosition = initialPos;
            element.gameObject.SetActive(false);
        }
        if (highlightSprite != null)
        {
            highlightSprite.gameObject.SetActive(false);
        }
        contentPanel.localScale = new Vector3(0.1f, contentPanel.localScale.y, contentPanel.localScale.z);
        StartCoroutine(StartOpenDelay());
    }
    private IEnumerator StartOpenDelay()
    {
        yield return new WaitForSeconds(initialDelaySeconds);
        OpenScroll();
    }
    public void OpenScroll()
    {
        // movimiento de la imagen en y  
        leftPost.DOAnchorPosY(finalY_LeftPost, openDuration).SetEase(Ease.OutQuad);
        rightPost.DOAnchorPosY(finalY_RightPost, openDuration).SetEase(Ease.OutQuad);
        // expande el contenido
        contentPanel.DOScaleX(1f, openDuration)
             .SetDelay(0.1f) 
             .SetEase(Ease.OutBack)
             .OnComplete(ShowButtons);
    }

    private void ShowButtons()
    {
        for (int i = 0; i < UI_Elements_To_Show.Count; i++)
        {
            RectTransform button = UI_Elements_To_Show[i];
            button.gameObject.SetActive(true);
            // movimiento del boton a pocicion final
            button.DOAnchorPos(originalButtonPositions[i], 0.3f)
                .SetDelay(i * buttonStaggerDelay)
                .SetEase(Ease.OutBack);
            // efecto rebote 
            button.DOScale(1.05f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
        }
        if (highlightSprite != null && UI_Elements_To_Show.Count > 0)
        {
            RectTransform firstButton = UI_Elements_To_Show[0];
            highlightSprite.gameObject.SetActive(true);
            // posición del primer botón
            highlightSprite.anchoredPosition = firstButton.anchoredPosition;
            // efecto 
            highlightSprite.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
        }
    }
}