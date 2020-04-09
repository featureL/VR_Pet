using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    public float speed = 10.0f;
    public int damage = 1;



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PenguinSphere")
        {
            Debug.Log("destroyed");
            Destroy(this.gameObject);
        }
    }
}
