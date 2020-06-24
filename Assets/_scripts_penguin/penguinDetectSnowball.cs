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
            //_penguinAI.hitReaction();
            //penguin.GetComponent<penguinModel>().launchNewScene("ThrowBallToPinguin");
            string action = penguin.GetComponent<penguinModel>().launchNewScene("ThrowBallToPinguin");
            //penguin.GetComponent<penguinAI>().PenguinActions("SadPenguinAfterHit");
            penguin.GetComponent<penguinAI>().PenguinActions(action + " ThrowBallToPinguin");

        }
        else if(other.tag == "PlayerFish")
        {
            Debug.Log("heheheheheheheheheheheh");
            GetComponent<penguinAI>().waitingForFish = false;
            //_penguinAI.hitReactionFish();
            //penguin.GetComponent<penguinModel>().launchNewScene("FeedPinguin");
            string action = penguin.GetComponent<penguinModel>().launchNewScene("FeedPinguin");
            Debug.Log(action);
            penguin.GetComponent<penguinAI>().PenguinActions(action + " FeedPinguin");
        }
        
    }

    public void hello()
    {

    }
}
