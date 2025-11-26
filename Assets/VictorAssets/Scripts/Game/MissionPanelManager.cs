using UnityEngine;

public class MissionPanelManager : MonoBehaviour
{
    public static MissionPanelManager Instance { get; private set; }

    [Header("Paneles de Misión")]

    [SerializeField] private GameObject troncoPanel;
    [SerializeField] private GameObject simbolosPanel;
    [SerializeField] private GameObject librosPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public GameObject GetPanel(string panelName)
    {
        switch (panelName)
        {
            case "Tronco":
                return troncoPanel;
            case "Simbolos":
                return simbolosPanel;
            case "Libros":
                return librosPanel;
            default:
                Debug.LogError($"Panel '{panelName}' no encontrado en MissionPanelManager.");
                return null;
        }
    }
}