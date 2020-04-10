using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PenguinSphere")
        {
            Debug.Log("destroyed");
            Destroy(this.gameObject);
        }
    }
}
