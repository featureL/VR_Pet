using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class setSnawBallTag : MonoBehaviour
{
    [SerializeField] private GameObject sphere;
    public void SetTeg()
    {
        Debug.Log("hahashsah");
        sphere.gameObject.tag = "PlayerSnowball";
    }
}
