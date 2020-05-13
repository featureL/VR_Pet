using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Tracking.Velocity;

public class testVelocity : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PenguinSphere")
        {
            Debug.Log("PenguinSphere");
        }
    }

}
