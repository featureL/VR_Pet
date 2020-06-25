using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishDestroyOnSphere : MonoBehaviour
{
    [SerializeField] private GameObject penguin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PenguinSphere" && this.tag == "PlayerFish")
        {
            Debug.Log("LODSFSDFSDFESD");
            penguin = GameObject.Find("Penguin");
            string action = penguin.GetComponent<penguinModel>().launchNewScene("FeedPinguin");
            penguin.GetComponent<penguinAI>().PenguinActions(action + " FeedPinguin");
            penguin.GetComponent<penguinAI>().waitingForFish = false;
            Debug.Log("FishHit from fish");
            Destroy(this.gameObject);
        }
    }
}
