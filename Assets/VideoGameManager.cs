using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SinglePlayer
{
    public class VideoGameManager : MonoBehaviour
    {
        public static VideoGameManager Instance;

        [Header("Settings")]
        public float fireMax = 100f;
        public float fireDrainRate = 1f;
        public float gameDuration = 240f;

        private float fireValue =100f;
        private float timerValue = 240f;

        public event Action<bool> OnGameEnded;
        private bool _gameEnded = false;
        private int cantidad;
        private int currentDead;
        public void AddFire(float amount)
        {
            fireValue = Mathf.Clamp(fireValue + amount, 0, fireMax);
        }
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Debug.Log("start");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            cantidad = PlayerPrefs.GetInt("JugadoresSeleccionados", 1);
        }
        public void UpdateDead()
        {
            currentDead++;
            Debug.Log(currentDead);
            Debug.Log(cantidad);
            if (cantidad== currentDead)
            {
                SceneManager.LoadScene("MenuSinglePlayer");
            }
        }
        private void Update()
        {
            //Debug.Log("update");

            fireValue -= fireDrainRate * Time.deltaTime;
            if (fireValue<= 0)
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

       

        private void EndGame(bool victory)
        {
            if (_gameEnded) return;
            _gameEnded = true;

            OnGameEnded?.Invoke(victory);

            ShowEndScreenClientRpc(victory);
        }

        [Rpc(SendTo.Everyone)]
        private void ShowEndScreenClientRpc(bool victory)
        {
            UIGameEnd.Instance.Show(victory);
        }

        [Rpc(SendTo.Server)]
        public void AddFireServerRpc(float amount)
        {
            fireValue = Mathf.Clamp(fireValue + amount, 0, fireMax);
        }
        public void Win()
        {

        }
        public float GetFire() => fireValue;
        public float GetTimer() => timerValue;
    }
}

