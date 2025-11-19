using UnityEngine;
using Unity.Netcode;
using System;

public class SpawnController : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPointArray;
    private int currentIndexArrayPosition;
    private void Reset()
    {
        gameObject.name = "SpawnController";
    }

    private void OnEnable()
    {
        GameManager.OnPositionPlayer += GetSpawnPoint;
    }
    private void OnDisable()
    {
        GameManager.OnPositionPlayer -= GetSpawnPoint;
    }
    public Vector3 GetSpawnPoint()
    {

        if (currentIndexArrayPosition < spawnPointArray.Length)
        {
            ++currentIndexArrayPosition;
            return spawnPointArray[currentIndexArrayPosition-1].position;
        }
        else
        {
            throw new Exception("Fuera De los limites");
        }
    }
}
