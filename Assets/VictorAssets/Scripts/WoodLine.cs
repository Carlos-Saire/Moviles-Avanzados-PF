using UnityEngine;
using UnityEngine.UI;
public class WoodLine : MonoBehaviour
{
    public bool isCut = false;
    private bool mouseOver = false;

    void Update()
    {
        if (mouseOver && Input.GetMouseButton(0)) 
        {
            isCut = true;
            GetComponent<Image>().color = Color.red; 
        }
    }

    public void OnPointerEnter()
    {
        mouseOver = true;
    }

    public void OnPointerExit()
    {
        mouseOver = false;
    }

}
