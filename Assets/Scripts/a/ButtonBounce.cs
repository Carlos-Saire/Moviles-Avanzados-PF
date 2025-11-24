using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonBounce : MonoBehaviour
{
    private Button button;
    private Transform target;

    [Header("Escalas")]
    [SerializeField] private float scaleUp = 1.2f;    // Cuánto crece
    [SerializeField] private float scaleDuration = 0.1f; // Velocidad del rebote

    private Vector3 originalScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        target = transform;
        originalScale = target.localScale;

        button.onClick.AddListener(() => StartCoroutine(Bounce()));
    }

    private IEnumerator Bounce()
    {
        // Escalar hacia arriba
        yield return StartCoroutine(ScaleTo(originalScale * scaleUp));

        // Volver a escalar hacia abajo
        yield return StartCoroutine(ScaleTo(originalScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 start = target.localScale;
        float time = 0f;

        while (time < scaleDuration)
        {
            time += Time.deltaTime;
            float t = time / scaleDuration;

            target.localScale = Vector3.Lerp(start, targetScale, t);
            yield return null;
        }

        target.localScale = targetScale;
    }
}
