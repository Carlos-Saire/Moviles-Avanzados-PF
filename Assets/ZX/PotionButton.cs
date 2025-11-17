using UnityEngine;

public class PotionButton : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private ColorGameManager manager;

    public void OnClick()
    {
        manager.AddIngredient(color);
    }
}
