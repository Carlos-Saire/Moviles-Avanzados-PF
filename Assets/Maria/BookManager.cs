using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BookManager : MonoBehaviour, IMiniGame
{
    [Header("Panel de la misión")]
    public GameObject missionPanel;

    private SinglePlayer.PlayerController currentPlayer;
    private MissionTrigger missionTrigger; // ahora guardamos MissionTrigger

    private bool completed = false;

    int objToCreate = 4;
    [SerializeField] Button[] booksInLibrary;
    [SerializeField] GameObject openPanelBook;
    [SerializeField] GameObject noteInBook;
    [SerializeField] GameObject noteParent;
    [SerializeField] Button closeBook;
    private bool[] hasNote;

    private GameObject[] noteInstances;
    private int currentBookIndex = -1;
    public int itemFounded = 0;
    public static event Action<int> OnNoteFound;

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

    void Start()
    {
        openPanelBook.SetActive(false);
        hasNote = new bool[booksInLibrary.Length];

        noteInstances = new GameObject[booksInLibrary.Length];
        GetRandomNote();
        for (int i = 0; i < booksInLibrary.Length; i++)
        {
            int k = i;
            booksInLibrary[i].onClick.AddListener(() => SetActive(k));
        }
        closeBook.onClick.AddListener(() => CloseBook());
    }

    void CloseBook()
    {
        if (currentBookIndex != -1 && noteInstances[currentBookIndex] != null)
        {
            noteInstances[currentBookIndex].SetActive(false);
        }
        currentBookIndex = -1;
        openPanelBook?.SetActive(false);
    }

    void SetActive(int index)
    {
        if (completed) return;

        if (currentBookIndex != -1 && noteInstances[currentBookIndex] != null)
            noteInstances[currentBookIndex].SetActive(false);

        openPanelBook.SetActive(true);
        currentBookIndex = index;
        if (hasNote[index])
        {
            if (noteInstances[index] == null)
            {
                noteInstances[index] = Instantiate(noteInBook, noteParent.transform);
                noteInstances[index].SetActive(true);
                Debug.Log(index + " tiene una nota (creada).");

                itemFounded++;
                OnNoteFound?.Invoke(itemFounded);

                if (itemFounded >= objToCreate)
                {
                    MissionCompleted();
                }
            }
            else
            {
                noteInstances[index].SetActive(true);
                Debug.Log(index + " tiene una nota (reutilizada).");
            }
        }
        else
        {
            Debug.Log(index + " está vacío.");
        }
    }

    void GetRandomNote()
    {
        int assigned = 0;
        while (assigned < objToCreate)
        {
            int randomIndex = UnityEngine.Random.Range(0, booksInLibrary.Length);
            if (!hasNote[randomIndex])
            {
                hasNote[randomIndex] = true;
                assigned++;
            }
        }
    }

    private void MissionCompleted()
    {
        completed = true;
        Debug.Log("Todas las notas encontradas! Misión completada.");
        StartCoroutine(ClosePanel());
    }

    private void ResetMission()
    {
        completed = false;
        itemFounded = 0;

        if (noteInstances == null)
            noteInstances = new GameObject[booksInLibrary.Length];

        for (int i = 0; i < noteInstances.Length; i++)
        {
            if (noteInstances[i] != null)
            {
                Destroy(noteInstances[i]);
                noteInstances[i] = null;
            }
        }

        hasNote = new bool[booksInLibrary.Length];

        GetRandomNote();

        CloseBook();
    }

    private IEnumerator ClosePanel()
    {
        MissionUIFeedback.Instance?.ShowMissionCompleted();

        yield return new WaitForSeconds(2f);

        if (missionPanel != null)
        {
            missionPanel.SetActive(false);
        }

        CloseBook();

        if (currentPlayer != null)
            currentPlayer.FreezePlayerSingle(false);
        currentPlayer.GetComponent<PlayerInput>().enabled = true;

        //if (VideoGameManager.Instance != null)
        //{
        //    VideoGameManager.Instance.AddFire(20f); // llamada local ahora
        //}

        // avisamos al MissionSpawnManager local
        if (missionTrigger != null)
        {
            MissionSpawnManager.Instance?.CompleteMission(missionTrigger);
        }
        else
        {
            Debug.LogWarning("BookManager: missionTrigger es null al completar la misión.");
        }
        var cursor = UnityEngine.Object.FindFirstObjectByType<UniversalGamepadCursorV2>(FindObjectsInactive.Include);
        if (cursor != null) cursor.EnableCursor(false);

        var playerInput = currentPlayer.GetComponent<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = true;
    }
}
