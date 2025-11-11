using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textPing;
    private void Reset()
    {
        gameObject.name = "UIManager";
    }
    private void Update()
    {
        textPing.text = GameManager.Instance.CalculatePing().ToString();
    }
}
