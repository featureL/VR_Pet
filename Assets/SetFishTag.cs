using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFishTag : MonoBehaviour
{
    [SerializeField] private GameObject fish;
    public void SetTeg()
    {
        Debug.Log("hahashsah");
        fish.gameObject.tag = "PlayerFish";
    }
}
