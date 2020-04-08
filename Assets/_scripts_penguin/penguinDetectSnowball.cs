using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguinDetectSnowball : MonoBehaviour
{
    [SerializeField] GameObject penguin;
    penguinAI _penguinAI;
    private void Start()
    {
        _penguinAI = penguin.GetComponent<penguinAI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //SnowBall snowball = other.GetComponent<SnowBall>();
        if(other.tag == "PlayerSnowball")
        {
            Debug.Log("Snowhit");
            _penguinAI.hitReaction();
        }
    }

    public void hello()
    {

    }
}
