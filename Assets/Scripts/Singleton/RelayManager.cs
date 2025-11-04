using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


public class RelayManager : MonoBehaviour
{
    public static RelayManager instance;

    [SerializeField] private SceneManagerController SceneManager;

    private void Reset()
    {
        gameObject.name="RelayManager";
    }
    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        await UnityServices.InitializeAsync();

        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public async Task<string> CreateRelay()
    {
        try
        {
            //List<Region> regions = await RelayService.Instance.ListRegionsAsync();

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = allocation.ToRelayServerData("dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            SceneManager.LoadScene("Lobby",LoadSceneMode.Single);

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error al crear Relay: " + e.Message);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            Debug.Log(joinCode);

            RelayServerData relayServerData = joinAllocation.ToRelayServerData("dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            
            SceneManager.LoadScene("Lobby");

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
