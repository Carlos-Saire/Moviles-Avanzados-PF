using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("PlayerInfoSo")]
    [SerializeField] private PlayerInfoSO playerSO;

    [Header("Lobby List")]
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private Button lobbyListButton;
    [SerializeField] private Transform lobbyListPanel;

    [Header("Create Lobby")]
    [SerializeField] private Transform createLobbyPanel;
    [SerializeField] private Transform editNamePanel; 
    [SerializeField] private TMP_Text currentNumberPlayersText;
    [SerializeField] private int maxPlayers = 4;
    [SerializeField] private Button buttonNext;
    [SerializeField] private Button buttonPrevious;
    [SerializeField] private Button buttonOpenPanelCreateLobby;
    [SerializeField] private Button buttonClousePanelCreateLobby;
    [SerializeField] private Button buttonClousePanelEditName; 
    [SerializeField] private Button buttonCreateLobby;
    [SerializeField] private TMP_Dropdown dropdownSegurity;
    private bool isPrivate;
    private void Reset()
    {
        gameObject.name = "LobbyUI";
    }
    private void OnEnable()
    {
        lobbyListButton?.onClick.AddListener(HandleButtonLobbyList);

        buttonOpenPanelCreateLobby?.onClick.AddListener(HandleButtonOpenLobby);
        buttonNext?.onClick.AddListener(HandleButtonNext);
        buttonPrevious?.onClick.AddListener(HandleButtonPrevious);
        buttonClousePanelCreateLobby?.onClick.AddListener(HandleButtonClousePanelCreateLobby);
        buttonClousePanelEditName?.onClick.AddListener(HandleButtonClousePanelEditName);
        buttonCreateLobby?.onClick.AddListener(HandlebuttonCreateLobby);
        dropdownSegurity?.onValueChanged.AddListener(OnDropdownCambiado);
    }

    private void OnDisable()
    {
        lobbyListButton?.onClick.RemoveListener(HandleButtonLobbyList);

        buttonOpenPanelCreateLobby?.onClick.RemoveListener(HandleButtonOpenLobby);
        buttonNext?.onClick.RemoveListener(HandleButtonNext);
        buttonPrevious?.onClick.RemoveListener(HandleButtonPrevious);
        buttonClousePanelCreateLobby?.onClick.RemoveListener(HandleButtonClousePanelCreateLobby);
        buttonCreateLobby?.onClick.RemoveListener(HandlebuttonCreateLobby);
        dropdownSegurity?.onValueChanged.RemoveListener(OnDropdownCambiado);


    }
    private void Start()
    {
        currentNumberPlayersText.text = maxPlayers.ToString();
        CheckButtonCreateLobby();
    }
    private void CreateLobby(Lobby lobby)
    {
        RectTransform newLobby = Instantiate(prefab);
        newLobby.SetParent(content);
        newLobby.localScale = Vector3.one;
        newLobby.GetComponent<LobbyInfo>().UpdateInformation(lobby);
    }
    private async void HandleButtonLobbyList()
    {
        QueryResponse lobbies = await LobbyManager.instance.ListLobbies();

        for (int i = 0; i < lobbies.Results.Count; ++i)
        {
            CreateLobby(lobbies.Results[i]);
        }

        lobbyListPanel.gameObject.SetActive(true);
    }

    private void HandleButtonOpenLobby()
    {
        createLobbyPanel.gameObject.SetActive(true);
        Debug.Log("Se abrio el panel para crear un lobby");
    }
    private void HandleButtonClousePanelCreateLobby()
    {
        createLobbyPanel.gameObject.SetActive(false);
        Debug.Log("Se Cerro el panel para crear un lobby");
    }
    private void HandleButtonClousePanelEditName()
    {
        editNamePanel.gameObject.SetActive(false);
        Debug.Log("Se Cerro el panel para crear un lobby");
    }

    private void HandleButtonPrevious()
    {
        --maxPlayers;
        currentNumberPlayersText.text = maxPlayers.ToString();
        CheckButtonCreateLobby();
    }

    private void HandleButtonNext()
    {
        ++maxPlayers;
        currentNumberPlayersText.text = maxPlayers.ToString();
        CheckButtonCreateLobby();
    }
    private void CheckButtonCreateLobby()
    {
        if (maxPlayers == 4)
        {
            buttonNext.interactable = false;
        }
        else if(maxPlayers == 1)
        {
            buttonPrevious.interactable = false;
        }
        else
        {
            buttonNext.interactable = true;
            buttonPrevious.interactable = true;
        }
    }
    private void HandlebuttonCreateLobby()
    {
        LobbyManager.instance.CreateLobby(playerSO.PlayerName, maxPlayers, isPrivate);
    }
    private void OnDropdownCambiado(int arg0)
    {
        isPrivate = arg0 == 1;
        Debug.Log("Valor booleano: " + isPrivate);
    }

}
