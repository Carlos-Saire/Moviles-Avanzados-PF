using UnityEngine;
using Unity.Services.Core;

public class UnityServicesManager : MonoBehaviour
{
    private void Reset()
    {
        gameObject.name = "UnityServicesManager";
    }
    public async void InitializeServices()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No hay conexión a internet. No se pueden inicializar Unity Services.");
            return;
        }
        await UnityServices.InitializeAsync();
        AuthenticationManager.Instance.InitializeAsync();
        Debug.Log("Se Inicializaron los servicios");
    }
    public bool AreServicesInitialized()
    {
        return UnityServices.State == ServicesInitializationState.Initialized;
    }
}
