using UnityEngine;

public class DeathUIManager : MonoBehaviour
{
    public static DeathUIManager Instance;

    [SerializeField] private GameObject deathPanel;

    private void Awake()
    {
        Instance = this;
        deathPanel.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        deathPanel.SetActive(true);
    }
}
