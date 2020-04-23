using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallDestroy : MonoBehaviour
{

    public void StartDestroy()
    {
        StartCoroutine(destroy());
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
