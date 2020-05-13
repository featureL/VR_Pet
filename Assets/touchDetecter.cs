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
             if (leftController.GetComponent<VelocityTracker>().GetVelocity().magnitude > 3)
             {
                if (!penguin.GetComponent<penguinAI>().Stop)
                {
                    penguin.GetComponent<penguinAI>().sad();
                }
            }
             else
            {
                if(!penguin.GetComponent<penguinAI>().Stop)
                {
                    penguin.GetComponent<penguinAI>().happy();
                }
            }
        }
    }
}
