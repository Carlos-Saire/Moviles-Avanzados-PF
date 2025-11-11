using UnityEngine;

public class WoodCuttingGame : MonoBehaviour
{
    public WoodLine[] lines;
    public GameObject panel;
    public GameObject fire;

    void Update()
    {
        if (AllLinesCut())
        {
            Debug.Log("¡Leña cortada!");
            fire.SetActive(true);
            panel.SetActive(false); 
        }
    }

    bool AllLinesCut()
    {
        foreach (var line in lines)
        {
            if (!line.isCut) return false;
        }
        return true;
    }

}
