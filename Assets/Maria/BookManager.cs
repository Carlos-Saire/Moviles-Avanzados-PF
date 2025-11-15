
using System;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
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
    void Start()
    {
        openPanelBook.SetActive(false);
        hasNote = new bool[booksInLibrary.Length];
       
        noteInstances = new GameObject[booksInLibrary.Length];
        GetRandomNote();
        for (int i =  0;  i < booksInLibrary.Length; i++)
        {
            int k = i;
           booksInLibrary[i].onClick.AddListener(() => SetActive(k));
        }
        closeBook.onClick.AddListener(() => CloseBook());
    }

    void Update()
    {
        if(itemFounded <= objToCreate)
        {
            Debug.Log("Notas encontradas: " + itemFounded + " de " + objToCreate);
            
        }
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
                Debug.Log(index + " tiene una nota (creada).");
                itemFounded++;
                OnNoteFound?.Invoke(itemFounded);
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
}
