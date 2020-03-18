using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBearMovement : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BearMovementLegacy bear;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))//Roar
            bear.Roar();
        else if(Input.GetKeyDown(KeyCode.Alpha2))//GetHit
            bear.GetHit();
        else if(Input.GetKeyDown(KeyCode.Alpha3))//Death
            bear.Death();
        else if(Input.GetKeyDown(KeyCode.Alpha4))//Attack
            bear.Attack();
        else if(Input.GetKeyDown(KeyCode.Alpha5))//Start Move
            bear.StartMove();
        else if(Input.GetKeyDown(KeyCode.Alpha6))//Stop Move
            bear.StopMove();
        else if(Input.GetKeyDown(KeyCode.Alpha7))//Change pose
            if(bear.IsStandOn2Legs())
                bear.StandOn4Legs();
            else
                bear.StandOn2Legs();
        else if(Input.GetKeyDown(KeyCode.Alpha8))//Output speed
            Debug.Log(bear.GetSpeed());
    }
}
