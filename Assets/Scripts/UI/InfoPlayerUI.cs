using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text birthday;
    [SerializeField] private Image profileImage;

    [SerializeField] private PlayerInfoSO playerInfoSO;
    private void Reset()
    {
        gameObject.name = "InfoPlayerUI";
    }
    public void UpdateData()
    {
        description.text = playerInfoSO.PlayerDescription;
        birthday.text = playerInfoSO.Playerbirthday;
        profileImage.sprite = playerInfoSO.GetProfileImage();
    }
}
