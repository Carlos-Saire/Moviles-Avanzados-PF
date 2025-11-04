using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField] private  Button buttonOpen;
    [SerializeField] private Button buttonBack;

    private Tween tween;
    private void OnEnable()
    {
        buttonOpen?.onClick.AddListener(Open);
        buttonBack?.onClick.AddListener(Back);
    }

    private void OnDisable()
    {
        buttonOpen?.onClick.RemoveListener(Open);
        buttonBack?.onClick.RemoveListener(Back);
    }
    private void Start()
    {
        
    }
    private void Open()
    {
        gameObject.SetActive(true);
    }
    private void Back()
    {
        gameObject.SetActive(false);
    }
}
