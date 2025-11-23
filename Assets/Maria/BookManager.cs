using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BookManager : MonoBehaviour
{
    [Header("Panel de la misión")]
    [SerializeField] GameObject missionPanel;  

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

    private PlayerController currentPlayer;  

    public void SetPlayer(PlayerController player)
    {
        currentPlayer = player;
    }

    void Start()
    {
        missionPanel.SetActive(false);
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

    private void OnEnable()
    {
        itemFounded = 0;
        currentBookIndex = -1;
        missionPanel.SetActive(true);
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

                itemFounded++;
                OnNoteFound?.Invoke(itemFounded);

                Debug.Log($"Libro {index} tiene nota. Notas encontradas {itemFounded}/{objToCreate}");
            }
            else
            {
                noteInstances[index].SetActive(true);
            }
        }
        else
        {
            Debug.Log($"Libro {index} está vacío.");
        }

        if (itemFounded >= objToCreate)
        {
            Debug.Log("MISIÓN COMPLETADA: Encontraste todas las notas!");
            StartCoroutine(FinishMission());
        }
    }

    IEnumerator FinishMission()
    {
        yield return new WaitForSeconds(2.5f);

        missionPanel.SetActive(false);

        if (currentPlayer != null)
            currentPlayer.FreezePlayer(false);

        VideoGameManager.Instance.AddFireServerRpc(20f);
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

        Debug.Log("Notas asignadas aleatoriamente.");
    }
}
