using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    [SerializeField] private GameObject sphere;
    private void OnTriggerEnter(Collider other)
    {
        if(sphere.gameObject.tag == "PlayerSnowball" && (other.name == "Environment" || other.tag == "PenguinSphere"))
        {
            Destroy(this.gameObject);
        }
    }
}
