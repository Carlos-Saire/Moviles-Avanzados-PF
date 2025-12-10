using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinController : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints; // <-- NUEVO

    private List<GameObject> jugadores = new List<GameObject>();
    private List<InputDevice> dispositivosUsados = new List<InputDevice>();

    private void Reset()
    {
        gameObject.name = "JoinController";
    }

    private void Start()
    {
        int cantidad = PlayerPrefs.GetInt("JugadoresSeleccionados", 1);

        for (int i = 0; i < cantidad; ++i)
        {
            CrearJugador();
        }
    }

    public void CrearJugador()
    {
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int index = jugadores.Count % spawnPoints.Length;
            spawnPos = spawnPoints[index].position;
            spawnRot = spawnPoints[index].rotation;
        }

        GameObject go = Instantiate(playerPrefab, spawnPos, spawnRot);
        jugadores.Add(go);
        ActualizarSplitScreen();
    }

    private InputDevice ObtenerSiguienteGamepadLibre()
    {
        foreach (var pad in Gamepad.all)
        {
            if (!dispositivosUsados.Contains(pad))
            {
                return pad;
            }
        }

        return null;
    }

    private void ActualizarSplitScreen()
    {
        int cantidad = jugadores.Count;

        for (int i = 0; i < cantidad; i++)
        {
            Camera cam = jugadores[i].GetComponentInChildren<Camera>();

            if (cam == null)
            {
                Debug.LogError("Jugador " + i + " no tiene cámara en el prefab.");
                continue;
            }

            cam.depth = i;

            switch (cantidad)
            {
                case 1:
                    cam.rect = new Rect(0f, 0f, 1f, 1f);
                    break;

                case 2:
                    cam.rect = (i == 0)
                        ? new Rect(0f, 0f, 0.5f, 1f)
                        : new Rect(0.5f, 0f, 0.5f, 1f);
                    break;

                case 3:
                    if (i == 0) cam.rect = new Rect(0f, 0f, 0.5f, 1f);
                    if (i == 1) cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    if (i == 2) cam.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                    break;

                case 4:
                    if (i == 0) cam.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                    if (i == 1) cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    if (i == 2) cam.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    if (i == 3) cam.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                    break;
            }
        }
    }
}