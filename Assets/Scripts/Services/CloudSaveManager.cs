using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
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
    private async void SaveProfile(PlayerJson profile)
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
    public async Task LoadProfileAsync()
    {
        try
        {
            var keys = new HashSet<string> { playerInfoSO.PlayerID };
            var result = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            if (!result.ContainsKey(playerInfoSO.PlayerID))
            {
                Debug.LogWarning("No profile found in Cloud Save.");
                return;
            }

            string json = result[playerInfoSO.PlayerID].Value.GetAsString();

            Debug.Log("Loaded JSON: " + json);

            PlayerJson loadedProfile = JsonUtility.FromJson<PlayerJson>(json);

            if (loadedProfile == null)
            {
                Debug.LogError("JSON could not be deserialized.");
                return;
            }

            playerInfoSO.PlayerDescription = loadedProfile.description;
            playerInfoSO.Playerbirthday = loadedProfile.birthday;
            playerInfoSO.PlayerIndexProfile = loadedProfile.profileIndex;

            Debug.Log("Profile loaded successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading profile: " + e.Message);
        }


    }
    private void OnApplicationQuit()
    {
        PlayerJson profile = new PlayerJson
        {
            description = playerInfoSO.PlayerDescription,
            birthday = playerInfoSO.Playerbirthday,
            profileIndex = playerInfoSO.PlayerIndexProfile
        };
        SaveProfile(profile);
    }
}
