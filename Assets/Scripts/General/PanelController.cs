using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField] private Button buttonOpen;
    [SerializeField] private Button buttonClouse;
    private void OnEnable()
    {
        buttonOpen.onClick.AddListener(Move);
    }
    private void OnDisable()
    {
        buttonOpen.onClick.RemoveListener(Move);
    }
    private void Move()
    {
        
    }
}
