using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelayManager : MonoBehaviour
{
    [SerializeField] private SceneManagerController SceneManager;

    private void Reset()
    {
        gameObject.name = "RelayManager";
    }
    public async Task<string> CreateRelay(int maxPlayers)
    {
        try
        {
            //List<Region> regions = await RelayService.Instance.ListRegionsAsync();

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers-1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = allocation.ToRelayServerData("dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error al crear Relay: " + e.Message);
            return null;
        }
    }

    public async Task JoinRelay(string joinCode)
    {
        try
        {

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

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
