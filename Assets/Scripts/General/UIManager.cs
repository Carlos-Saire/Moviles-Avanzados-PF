using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text fireText;

    [Header("Panels")]
    [SerializeField] private GameObject fail;
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject gameTronco;

    [Header("Cursor")]
    [SerializeField] private UniversalGamepadCursorV2 cursor;

    [Header("Player")]
    [SerializeField] private SinglePlayer.PlayerController playerController;
    [SerializeField] private SinglePlayer.CameraController cameraController;
    private void OnEnable()
    {
        UIVideoGame.OnFireChanged += UpdateFireUI;
        UIVideoGame.OnTimeChanged += UpdateTimerUI;

        TroncoMiniGame.OnMissionCompleted += FinishGame;
    }
    private void OnDisable()
    {
        UIVideoGame.OnFireChanged -= UpdateFireUI;
        UIVideoGame.OnTimeChanged -= UpdateTimerUI;

        TroncoMiniGame.OnMissionCompleted -= FinishGame;
    }
    private void UpdateTimerUI(int arg1, int arg2)
    {
        timerText.text = $"{arg1:00}:{arg2:00}";
    }

    private void UpdateFireUI(float obj)
    {
        fireText.text=Mathf.RoundToInt(obj) + "%";
    }
    public void Fail()
    {
        fail.gameObject.SetActive(true);
    }
    public void Game()
    {
        playerController.isFrozen = true;
        cameraController.isFrozen = true;

        gameTronco.gameObject.SetActive(true);
        cursor.EnableCursor(true);
    }
    private void FinishGame()
    {
        Debug.Log("Se llamo");
        cursor.EnableCursor(false);

        playerController.isFrozen = false;
        cameraController.isFrozen = false;
    }

}
