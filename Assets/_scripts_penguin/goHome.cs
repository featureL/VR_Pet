using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class goHome : MonoBehaviour
{

    public GameObject penguinHome;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            //_ai = GetComponent<penguinAI>();
            //_ai.enabled = false;
            GetComponent<penguinAI>().Move(penguinHome.transform.position);
        }
    }
}
