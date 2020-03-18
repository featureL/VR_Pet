using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovementLegacy : MonoBehaviour
{
    private Animation anim;

    private enum State
    { Death, Idle, Move };
    private State state = State.Idle;
    private enum Pose {TwoLegs = 2, FourLegs = 4 };
    private Pose pose = Pose.FourLegs;

    //триггеры
    private bool getHitTrigger = false;
    private bool roarTrigger = false;
    private bool attackTrigger = false;
    private bool deathTrigger = false;
    private bool changePoseTo4LegsTrigger = false;
    private bool changePoseTo2LegsTrigger = false;

    private bool startMoveTrigger = false;
    private bool stopMoveTrigger = false;

    //"0" устанавливается при "idle", "1" соответствует "walk", а "2" соответствует "run"
    //При значениях скорости между 1 и 2 происходит их смещивание (но это не точно)
    private float speed;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        ChangeState();
       // Debug.Log(state);
       // Debug.Log(pose);
        /*Debug.Log(getHitTrigger);
        Debug.Log(deathTrigger);
        Debug.Log(changePoseTo2LegsTrigger);
        Debug.Log(changePoseTo4LegsTrigger);
        Debug.Log(attackTrigger);*/
    }

    public void Roar()
    { roarTrigger = true; }

    public void GetHit()
    { getHitTrigger = true; }

    public void Death()
    { deathTrigger = true; }

    public void StandOn2Legs()
    { changePoseTo2LegsTrigger = true; }

    public void StandOn4Legs()
    { changePoseTo4LegsTrigger = true; }

    public void Attack()
    { attackTrigger = true; }

    public void StartMove()
    { startMoveTrigger = true; }

    public void StopMove()
    { stopMoveTrigger = true; }

    public bool IsStandOn2Legs()
    { return (pose == Pose.TwoLegs); }

    public void SetSpeed(float speed)
    {
        if (speed > 2)
            this.speed = 2;
        else if (speed < 1)
            this.speed = 1;
        else
            this.speed = speed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    private void ChangeState()
    {
        if(roarTrigger) {
            roarTrigger = false;
            if(state != State.Death) {
                state = State.Idle;
                if(pose == Pose.TwoLegs) {
                    anim.Play("2LegsRoar");
                }
                else {
                    anim.Play("4LegsRoar");
                }
            }
        }
        if(attackTrigger) {
            attackTrigger = false;
            if(state != State.Death) {
                state = State.Idle;
                int randomValue = Mathf.FloorToInt(Random.Range(0, 3));
                switch (randomValue)
                {
                    case 0:
                        if (pose == Pose.TwoLegs)
                            anim.Play("2LegsClawsAttackL");
                        else
                            anim.Play("4LegsClawsAttackL");
                        break;
                    case 1:
                        if (pose == Pose.TwoLegs)
                            anim.Play("2LegsClawsAttackR");
                        else
                            anim.Play("4LegsClawsAttackR");
                        break;
                    case 2:
                        if (pose == Pose.TwoLegs)
                            anim.Play("2LegsBiteAttack");
                        else
                            anim.Play("4LegsBiteAttack");
                        break;
                }
            }
        }
        if(changePoseTo2LegsTrigger) {
            changePoseTo2LegsTrigger = false;
            if(state != State.Death) {
                state = State.Idle;
                if(pose == Pose.FourLegs) {
                    anim.Play("4LegsTo2Legs");
                    pose = Pose.TwoLegs;
                }
            }
        }
        if(changePoseTo4LegsTrigger) {
            changePoseTo4LegsTrigger = false;
            if(state != State.Death) {
                state = State.Idle;
                if(pose == Pose.TwoLegs) {
                    anim.Play("2LegsTo4Legs");
                    pose = Pose.FourLegs;
                }
            }
        }
        if(startMoveTrigger) {
            startMoveTrigger = false;
            if(state != State.Death && pose == Pose.FourLegs) {
                state = State.Move;
                anim.Stop();
            }
        }
        if(stopMoveTrigger) {
            stopMoveTrigger = false;
            if(state != State.Death) {
                state = State.Idle;
                anim.Stop();
            }
        }
        if(getHitTrigger) {
            getHitTrigger = false;
            if(state != State.Death) {
                state = State.Idle;
                if(pose == Pose.TwoLegs) {
                    anim.Play("2LegsGetHit");
                }
                else {
                    anim.Play("4LegsGetHit");
                }
            }
        }
        if(deathTrigger)
        {
            deathTrigger = false;
            if(state != State.Death) {
                state = State.Death;
                if(pose == Pose.TwoLegs) {
                    anim.Play("2LegsDeath");
                }
                else {
                    anim.Play("4LegsDeath");
                }
            }
        }
        if(state == State.Move)
        {
            if(pose == Pose.FourLegs) {
                anim.PlayQueued("walk");
            }
        }
        if(state == State.Idle)
        {
            if(pose == Pose.TwoLegs) {
                anim.PlayQueued("idle2Legs");
            }
            else {
                anim.PlayQueued("idle4Legs");
            }
        }
    }
}
