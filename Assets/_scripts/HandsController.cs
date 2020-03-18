using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    public static HandsController instance;

    void Start()
    {
        instance = this;
        InvokeRepeating("CheckHands", 5, 4);
    }

    public static bool isHoldingTorch = false;
    public static bool isHoldingFish = false;
    public static bool isHoldingSnowball = false;

    public Transform torch;

    public List<GameObject> objectsInHands = new List<GameObject>();

    public void CheckHands()
    {
        if(objectsInHands.Count == 0)
        {
            isHoldingSnowball = false;
            isHoldingFish = false;
            isHoldingTorch = false;
        }

        foreach (GameObject item in objectsInHands)
        {
            if (item.GetComponent<ObjType>().typeOfInteractable == ObjType.TypeOfInteractable.Fish)
            {
                isHoldingFish = true;
                break;
            } else
            {
                isHoldingFish = false;
            }
            
        }


        foreach (GameObject item in objectsInHands)
        {
            if (item.GetComponent<ObjType>().typeOfInteractable == ObjType.TypeOfInteractable.Torch)
            {
                torch = item.transform;
                isHoldingTorch = true;
                break;
            }
            else
            {
                torch = null;
                isHoldingTorch = false;
            }

        }

        foreach (GameObject item in objectsInHands)
        {
            if (item.GetComponent<ObjType>().typeOfInteractable == ObjType.TypeOfInteractable.Snowball)
            {
                isHoldingSnowball = true;
                break;
            }
            else
            {
                isHoldingSnowball = false;
            }

        }
    }

}
