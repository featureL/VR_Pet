using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spownFish : MonoBehaviour
{
    [SerializeField] private GameObject fish;
    private GameObject _fish1;
    [SerializeField] private GameObject SpawnFishPoint;

    public void SpownFish()
    {
        Debug.Log("getting fish");
        _fish1 = Instantiate(fish) as GameObject;
        _fish1.transform.position = SpawnFishPoint.transform.position;
    }
}
