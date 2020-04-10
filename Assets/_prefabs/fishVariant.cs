using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishVariant : MonoBehaviour
{
    public float speed = 4.0f;
    public int damage = 1;


    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    //Debug.Log("destroyed");
    //    StartCoroutine(destroyFish());

    //}
    //IEnumerator destroyFish()
    //{
    //    yield return new WaitForSeconds(2);
    //    Destroy(this.gameObject);
    //}
}
