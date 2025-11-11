using UnityEngine;
using Unity.Netcode;
using System;

[RequireComponent(typeof(NetworkObject))]
public class SpawnController : NetworkBehaviour
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
            Debug.Log(currentIndexArrayPosition - 1);
            return spawnPointArray[currentIndexArrayPosition-1].position;
        }
        else
        {
            throw new Exception("Fuera De los limites");
        }

    }
}
