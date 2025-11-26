using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class BookManager : MonoBehaviour
{
    [Header("Panel de la misión")]
    public GameObject missionPanel; 

    private PlayerController currentPlayer; 
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

    public void SetPlayer(PlayerController pc)
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
        yield return new WaitForSeconds(2.5f);

        if (missionPanel != null)
        {
            missionPanel.SetActive(false);
        }

        CloseBook();

        if (currentPlayer != null)
            currentPlayer.FreezePlayer(false); 

        if (VideoGameManager.Instance != null)
        {
            VideoGameManager.Instance.AddFireServerRpc(20f); 
        }
    }
}