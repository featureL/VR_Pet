using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public void PickedUpObject()
    {
        HandsController.instance.objectsInHands.Add(gameObject);
    }

    public void DetachedObject()
    {
        HandsController.instance.objectsInHands.Remove(gameObject);
    }
}
