using UnityEngine;
using System;

public class VideoGameManager : MonoBehaviour
{
    public static VideoGameManager Instance;

    [Header("Settings")]
    public float fireMax = 100f;
    public float fireDrainRate = 1f;
    public float gameDuration = 240f;

    private float fireValue = 100f;
    private float timerValue = 240f;

    public event Action<bool> OnGameEnded;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        // En singleplayer, todo corre en el Update local
        fireValue -= fireDrainRate * Time.deltaTime;
        if (fireValue <= 0)
        {
            fireValue = 0;
            Debug.Log("GAME OVER: El fuego se apagó");
            EndGame(false);
        }

        timerValue -= Time.deltaTime;
        if (timerValue <= 0)
        {
            timerValue = 0;
            Debug.Log("VICTORY: Sobrevivieron los 4 minutos");
            EndGame(true);
        }
    }

    private bool _gameEnded = false;

    private void EndGame(bool victory)
    {
        if (_gameEnded) return;
        _gameEnded = true;

        OnGameEnded?.Invoke(victory);

        // Mostrar UI local
        UIGameEnd.Instance.Show(victory);
    }

    // Reemplazo del ServerRpc: método local
    public void AddFire(float amount)
    {
        fireValue = Mathf.Clamp(fireValue + amount, 0, fireMax);
    }

    public float GetFire() => fireValue;
    public float GetTimer() => timerValue;
}
