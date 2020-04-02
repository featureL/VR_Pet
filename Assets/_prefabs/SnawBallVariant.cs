using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnawBallVariant : MonoBehaviour
{
    public float speed = 10.0f;
    public int damage = 1;


    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log("destroyed");
        StartCoroutine(destroySnawBall());

    }
    IEnumerator destroySnawBall()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
