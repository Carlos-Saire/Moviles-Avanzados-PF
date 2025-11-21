using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private PlayerInfoSO playerInfoSO;

    public static CloudSaveManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public async void SaveProfile(PlayerJson profile)
    {
        string json = JsonUtility.ToJson(profile);

        var data = new Dictionary<string, object>
        {
            { playerInfoSO.PlayerID, json }
        };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        Debug.Log("Profile saved to Cloud Save!");
    }
    public async Task<bool> IsFirstTimePlayer(string playerID)
    {
        var keys = new HashSet<string> { playerID };

        try
        {
            var result = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            if (!result.ContainsKey(playerID))
            {
                Debug.Log("Player profile does not exist. First time player.");
                return true;
            }

            Debug.Log("Player profile found. Returning player.");
            return false;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error checking player profile: " + ex.Message);
            return true;
        }
    }
}
