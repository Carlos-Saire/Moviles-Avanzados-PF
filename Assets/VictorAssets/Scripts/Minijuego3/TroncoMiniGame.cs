using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TroncoMiniGame : MonoBehaviour, IMiniGame
{
    public SinglePlayer.VideoGameManager game;
    [Header("Panel de la misión")]
    public GameObject missionPanel;

    [Header("Tronco")]
    public Image logImage;
    public Sprite logCutSprite;
    private Sprite originalLogSprite;

    [Header("Lineas a cortar")]
    public Image[] cutLines;

    private int linesLeft;
    private bool completed = false;

    private SinglePlayer.PlayerController currentPlayer;
    private MissionTrigger missionTrigger;
    public static event Action OnMissionCompleted;
    private void Awake()
    {
        originalLogSprite = logImage.sprite;
    }
    private void Start()
    {
        if (game == null)
        {
            game = FindFirstObjectByType<SinglePlayer.VideoGameManager>(FindObjectsInactive.Include);
            if (game == null)
                Debug.LogError("TroncoMiniGame NO encontró VideoGameManager");
        }
    }

    public void SetMissionObject(MissionTrigger missionObj)
    {
        missionTrigger = missionObj;
    }
    public void SetPlayer(SinglePlayer.PlayerController pc)
    {
        currentPlayer = pc;
    }
    private void OnEnable()
    {
        ResetMission();
    }

    private void ResetMission()
    {
        linesLeft = cutLines.Length;
        completed = false;

        logImage.sprite = originalLogSprite;

        for (int i = 0; i < cutLines.Length; i++)
        {
            cutLines[i].gameObject.SetActive(true);

            int index = i;
            cutLines[i].GetComponent<Button>().onClick.RemoveAllListeners();
            cutLines[i].GetComponent<Button>().onClick.AddListener(() => CutLine(index));
        }
    }

    private void CutLine(int index)
    {
        cutLines[index].gameObject.SetActive(false);
        linesLeft--;

        if (linesLeft <= 0 && !completed)
        {
            completed = true;
            logImage.sprite = logCutSprite;

            StartCoroutine(CloseMissionPanel());
        }
    }

    private IEnumerator CloseMissionPanel()
    {
        MissionUIFeedback.Instance?.ShowMissionCompleted();
        yield return new WaitForSeconds(2f);
        missionPanel.SetActive(false);

        Debug.Log(">>> CloseMission: des-congelando jugador");
        OnMissionCompleted?.Invoke();
        //if (currentPlayer != null)
        //{
        //    currentPlayer.FreezePlayerSingle(false);

        //    var input = currentPlayer.GetComponent<PlayerInput>();
        //    if (input != null)
        //        input.enabled = true;
        //}

        game.AddFire(10f);
        Debug.Log(">>> CloseMission: completando misión");
        if (MissionSpawnManager.Instance == null)
            Debug.LogError("❌ MissionSpawnManager.Instance ES NULL");

        //if (missionTrigger == null)
        //    Debug.LogError("❌ missionTrigger ES NULL en CloseMission");

        //MissionSpawnManager.Instance?.CompleteMission(missionTrigger);

        //Debug.Log(">>> CloseMission: apagando cursor");

        //var cursor = UnityEngine.Object.FindFirstObjectByType<UniversalGamepadCursorV2>(FindObjectsInactive.Include);
        //if (cursor != null)
        //{
        //    cursor.EnableCursor(false);
        //}
        //missionTrigger.CompleteMission(currentPlayer);
        //Debug.Log(">>> CloseMission: FIN");
    }
}
