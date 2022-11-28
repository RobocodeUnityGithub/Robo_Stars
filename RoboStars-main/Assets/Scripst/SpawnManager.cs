using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    private SpawnPoint[] spawns;
    private void Awake()
    {
        Instance = this;
        spawns = GetComponentsInChildren<SpawnPoint>();
    }
    public Transform GetSpawPoint()
    {
        return spawns[Random.Range(0, spawns.Length)].transform;
    }
}
