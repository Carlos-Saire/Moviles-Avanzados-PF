using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinController : MonoBehaviour
{
    public PlayerInputManager inputManager;

    [Header("Prefabs de Jugadores")]
    public GameObject playerPrefab;

    private List<PlayerInput> jugadores = new List<PlayerInput>();

    void Awake()
    {
        inputManager.enabled = false; 
    }
    private void Start()
    {
        
    }
    public void CrearJugador()
    {
        PlayerInput jugador = PlayerInput.Instantiate(
            playerPrefab,
            jugadores.Count,         
            null,
            -1,
            Keyboard.current          
        );

        jugadores.Add(jugador);
        ActualizarSplitScreen();
    }

    private void ActualizarSplitScreen()
    {
        int cantidad = jugadores.Count;

        for (int i = 0; i < cantidad; i++)
        {
            Camera cam = jugadores[i].GetComponentInChildren<Camera>();

            if (cam == null)
            {
                Debug.LogWarning("El prefab no contiene cámara. Agrégale una al jugador.");
                continue;
            }

            switch (cantidad)
            {
                case 1:
                    cam.rect = new Rect(0, 0, 1, 1);
                    break;

                case 2:
                    cam.rect = (i == 0)
                        ? new Rect(0, 0.5f, 1, 0.5f)   
                        : new Rect(0, 0, 1, 0.5f);     
                    break;

                case 3:
                case 4:
                    if (i == 0) cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    if (i == 1) cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    if (i == 2) cam.rect = new Rect(0, 0, 0.5f, 0.5f);
                    if (i == 3) cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
            }
        }
    }
}
