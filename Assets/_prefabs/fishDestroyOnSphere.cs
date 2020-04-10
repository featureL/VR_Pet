using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishDestroyOnSphere : MonoBehaviour
{
    [SerializeField] private GameObject penguin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PenguinSphere")
        {
            penguin = GameObject.Find("Penguin");
            penguin.GetComponent<penguinAI>().hitReactionFish();
            Debug.Log("FishHit from fish");
            Destroy(this.gameObject);
        }
    }
}
