using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Tracking.Velocity;

public class touchDetecter : MonoBehaviour
{
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    [SerializeField] private GameObject penguin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactor")
        {
             if (leftController.GetComponent<VelocityTracker>().GetVelocity().magnitude > 5)
             {
                if (!penguin.GetComponent<penguinAI>().Stop)
                {
                    //penguin.GetComponent<penguinAI>().sad();
                    string action = penguin.GetComponent<penguinModel>().launchNewScene("HitPinguin");
                    penguin.GetComponent<penguinAI>().PenguinActions("SadPenguinAfterHit");

                }
            }
             else
            {
                if(!penguin.GetComponent<penguinAI>().Stop)
                {
                    //penguin.GetComponent<penguinAI>().happy();
                    string action = penguin.GetComponent<penguinModel>().launchNewScene("StrokePinguin");
                    penguin.GetComponent<penguinAI>().PenguinActions(action + " StrokePinguin");
                }
            }
        }
    }
}
