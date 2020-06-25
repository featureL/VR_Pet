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
        
        if(other.tag == "PlayerSnowball")
        {
            string action = penguin.GetComponent<penguinModel>().launchNewScene("ThrowBallToPinguin");
            penguin.GetComponent<penguinAI>().PenguinActions(action + " ThrowBallToPinguin");

        }
        else if(other.tag == "PlayerFish")
        {
            Debug.Log("heheheheheheheheheheheh");
            GetComponent<penguinAI>().waitingForFish = false;
            string action = penguin.GetComponent<penguinModel>().launchNewScene("FeedPinguin");
            Debug.Log(action);
            penguin.GetComponent<penguinAI>().PenguinActions(action + " FeedPinguin");
        }
        
    }

    public void hello()
    {

    }
}
