using UnityEngine;
[CreateAssetMenu(fileName = "PlayerInfoSO", menuName = "ScriptableObject/PlayerInfoSO", order =1)]
public class PlayerInfoSO : ScriptableObject
{
    public string PlayerName { get; set; }
}
