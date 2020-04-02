using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class turnToThePlayer:MonoBehaviour
{
    public GameObject Player;
    private Transform target;
    private float timer;
    private bool isCoroutineExecuting = false;
    private bool anim = false;
    public float speed = 1.0f;
    void Start()
    {
        target = Player.GetComponent<Transform>();
    }



    void Update()
    {
        if(!anim)
        {
            GetComponent<Animator>().SetBool("isSleeping", false);
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            anim = true;
        }
        timer += Time.deltaTime;
        if (timer > 3 && !isCoroutineExecuting)
        {
            StartCoroutine(lookingAtThePlayer());
        }
        else if(timer < 3)
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = target.position - transform.position;
            Debug.Log(targetDirection);
            Debug.Log("-");
            Debug.Log(transform.forward);
            // The step size is equal to speed times frame time.
            float singleStep = speed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
            Debug.Log(newDirection);
        }

        IEnumerator lookingAtThePlayer()
        {
            if(isCoroutineExecuting)
                yield break;
            isCoroutineExecuting = true;
            GetComponent<Animator>().SetBool("isStaying", false);
            GetComponent<Animator>().SetBool("isWaving",true);
            yield return new WaitForSeconds(1);
            GetComponent<penguinAI>().enabled = true;
            GetComponent<Animator>().SetBool("isWaving", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            isCoroutineExecuting = false;
            timer = 0;
            anim = false;
            GetComponent<penguinAI>().Stop = false;
            this.enabled = false;
        }


    }
}
