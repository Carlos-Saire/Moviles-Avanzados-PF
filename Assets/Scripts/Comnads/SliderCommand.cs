using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SliderCommand : ICommand
{
    private Slider slider;
    private float duration;

    public SliderCommand(Slider slider, float duration)
    {
        this.slider = slider;
        this.duration = duration;
    }
    public IEnumerator Execute()
    {
        float elapsed = 0f;
        slider.value = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        slider.value = 1f;
    }
}
