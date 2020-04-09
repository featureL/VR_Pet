using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setSnawBallTag : MonoBehaviour
{
    public void SetTeg()
    {
        GetComponent<GameObject>().gameObject.tag = "PlayerSnowball";
    }
}
