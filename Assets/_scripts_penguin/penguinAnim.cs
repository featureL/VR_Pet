using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguinAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animation _anim = GetComponent<Animation>();
        _anim.Play("First");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
