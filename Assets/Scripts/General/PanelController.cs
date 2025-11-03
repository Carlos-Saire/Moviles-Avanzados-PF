using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField] private  Button buttonOpen;
    [SerializeField] private Button buttonClouse;

    private Tween tween;
    private void OnEnable()
    {
        buttonOpen?.onClick.AddListener(Open);
        buttonClouse?.onClick.AddListener(Clouse);
    }

    private void OnDisable()
    {
        buttonOpen?.onClick.RemoveListener(Open);
        buttonClouse?.onClick.RemoveListener(Clouse);
    }
    private void Start()
    {
        
    }
    private void Open()
    {

    }
    private void Clouse()
    {

    }
}
