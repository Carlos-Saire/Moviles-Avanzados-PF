using TMPro;
using UnityEngine;

public class TextVivoxInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text textInormation;
    private void Reset()
    {
        gameObject.name = "TextVivox";
    }
    public void UpdateInformation(string text)
    {
        textInormation.text = text;
    }
}
