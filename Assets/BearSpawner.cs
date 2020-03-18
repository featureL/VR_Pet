using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    public GameObject bearPrefab;

    public void Spawn()
    {
        Instantiate(bearPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
    }

    public bool badBoolSpawn = false;

    private void Update()
    {
        if (badBoolSpawn)
        {
            badBoolSpawn = false;
            Spawn();
        }

    }
}
