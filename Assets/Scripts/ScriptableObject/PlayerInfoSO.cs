using UnityEngine;
[CreateAssetMenu(fileName = "PlayerInfoSO", menuName = "ScriptableObject/PlayerInfoSO", order =1)]
public class PlayerInfoSO : ScriptableObject
{
    public string PlayerName { get; set; }
    public string PlayerID { get; set; }
    public string PlayerDescription { get; set; }
    public string Playerbirthday { get; set; }
    public int PlayerIndexProfile { get; set; }
    [SerializeField] private Sprite[] profileImages;
    public int numbOfPlayers { get; set; }
    public Sprite GetProfileImage()
    {
        return profileImages[PlayerIndexProfile];
    }
}
