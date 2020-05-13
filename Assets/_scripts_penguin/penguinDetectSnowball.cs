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
            Debug.Log(other.tag);
            _penguinAI.hitReaction();
        }
        else if(other.tag == "PlayerFish")
        {
            Debug.Log("Fishhit");
            GetComponent<penguinAI>().waitingForFish = false;
            Debug.Log("penguinDetectSnawBall " + GetComponent<penguinAI>().waitingForFish);
            _penguinAI.hitReactionFish();
        }
        
    }

    public void hello()
    {

    }
}
