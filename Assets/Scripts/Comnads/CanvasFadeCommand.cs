using System.Collections;
using UnityEngine;

public class CanvasFadeCommand : ICommand
{
    private readonly CanvasGroup _canvasGroup;
    private readonly float _newAlpha;
    private readonly float _duration;

    public CanvasFadeCommand(CanvasGroup canvasGroup, float newAlpha, float duration)
    {
        _canvasGroup = canvasGroup;
        _newAlpha = newAlpha;
        _duration = duration;
    }

    public IEnumerator Execute()
    {
        float initialAlpha = _canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _duration;
            _canvasGroup.alpha = Mathf.Lerp(initialAlpha, _newAlpha, t);
            yield return null;
        }

        _canvasGroup.alpha = _newAlpha; 

    }
}
