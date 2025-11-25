using UnityEngine;
using UnityEngine.UI;

public class ProfileImageSelector : MonoBehaviour
{
    public static Button SelectedButton;
    public static int? SelectedImageIndex;
    private Button button;

    [SerializeField] private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private int imageIndex;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonPress);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(ButtonPress);
    }
    private void ButtonPress()
    {
        if(SelectedButton != null&& SelectedButton != button)
            SelectedButton.gameObject.transform.localScale = Vector3.one;

        if (SelectedButton != button)
        {
            SelectedButton = button;
            button.gameObject.transform.localScale = selectedScale;
            SelectedImageIndex = imageIndex;
        }
    }
}
