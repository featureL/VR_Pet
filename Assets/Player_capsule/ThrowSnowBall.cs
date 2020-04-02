using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSnowBall : MonoBehaviour
{
    [SerializeField] private GameObject snowBall;
    private GameObject _snowball;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
                _snowball = Instantiate(snowBall) as GameObject;
                _snowball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                _snowball.transform.rotation = transform.rotation;
        }

    }
}
