using Unity.Netcode;
using UnityEngine;

public class VideoGameManager : NetworkBehaviour
{
    public static VideoGameManager Instance;

    [Header("Settings")]
    public float fireMax = 100f;
    public float fireDrainRate = 1f; 
    public float gameDuration = 240f; 

    private NetworkVariable<float> fireValue = new(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<float> timerValue = new(240f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsServer) return;


        fireValue.Value -= fireDrainRate * Time.deltaTime;
        if (fireValue.Value <= 0)
        {
            fireValue.Value = 0;
            Debug.Log("GAME OVER: El fuego se apagó");
        }


        timerValue.Value -= Time.deltaTime;
        if (timerValue.Value <= 0)
        {
            timerValue.Value = 0;
            Debug.Log("VICTORY: Sobrevivieron los 4 minutos");
        }
    }

    public float GetFire() => fireValue.Value;
    public float GetTimer() => timerValue.Value;

    [Rpc(SendTo.Server)]
    public void AddFireServerRpc(float amount)
    {
        fireValue.Value = Mathf.Clamp(fireValue.Value + amount, 0, fireMax);
    }
}
