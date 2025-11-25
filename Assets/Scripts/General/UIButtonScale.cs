using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 normalScale;

    private void Awake()
    {
        normalScale = transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = selectedScale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = normalScale;
    }
}
