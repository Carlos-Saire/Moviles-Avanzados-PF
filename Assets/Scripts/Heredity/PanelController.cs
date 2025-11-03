using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class PanelController : MonoBehaviour
{
    [SerializeField] protected  Button buttonOpen;
    [SerializeField] protected Button buttonClouse;
    protected void OnEnable()
    {
        buttonOpen.onClick.AddListener(Open);
        buttonClouse.onClick.AddListener(Clouse);
    }

    protected void OnDisable()
    {
        buttonOpen.onClick.RemoveListener(Open);
        buttonClouse.onClick.RemoveListener(Clouse);
    }
    protected void Open()
    {
        
    }
    protected void Clouse()
    {

    }
}
