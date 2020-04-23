using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForFish : MonoBehaviour
{
    public void findPenguin()
    {
        GameObject penguin = GameObject.Find("Penguin");
        penguin.GetComponent<penguinAI>().WaitForFish();
    }

}
