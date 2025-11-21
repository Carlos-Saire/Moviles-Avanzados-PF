using UnityEngine;
using Unity.Services.Core;

public class UnityServicesManager : MonoBehaviour
{
    public async void InitializeServices()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No hay conexión a internet. No se pueden inicializar Unity Services.");
            return;
        }
        await UnityServices.InitializeAsync();
        Debug.Log("Se Inicializaron los servicios");
    }
    public bool AreServicesInitialized()
    {
        return UnityServices.State == ServicesInitializationState.Initialized;
    }
}
