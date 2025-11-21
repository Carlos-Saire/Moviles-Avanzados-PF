using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerID;
    [SerializeField] private Image profileImage;

    [Header("PlayerInfoSO")]
    [SerializeField] PlayerInfoSO playerInfoSO;
    private void Reset()
    {
        gameObject.name = "MenuUI";
    }
    private void Start()
    {
        playerName.text = "Name : " +playerInfoSO.PlayerName;
        playerID.text = "ID : " + playerInfoSO.PlayerID;
        profileImage.sprite = playerInfoSO.GetProfileImage();
    }
}
