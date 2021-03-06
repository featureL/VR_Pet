﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;

public class penguinAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private GameObject snowBall;
    private GameObject _snowball;
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject fishEating;
    private GameObject _fish;
    private GameObject _fishEating;
    [SerializeField] private GameObject fishPosition;


    private int _layermask = -1;
    public float _maxDistance = 12;
    
    private Vector3 _origin;
    
    private bool isCoroutineExecuting = false;
    private bool isCoroutineExecutingSleep = false;
    private bool isCoroutineExecutingHungry = false;
    private bool isCoroutineExecutingStayingInFrontOfPlayer = false;
    private bool isCoroutineExecutingPlayful = false;
    public bool isSleep = false;
    public bool Stop = false;
    private bool isHungry = false;
    private bool isPlayful = false;
    private bool isTurning = false;
    private bool isStayingInFrontOfPlayer = false;
    public bool waitingForFish = false;
    private bool isWalking = false;
    private bool throwToPlayer = false;
    private bool eatFish = false;
    private Vector3 _lastDestination;


    private float _stayTime = 5.0f;

    [SerializeField] private GameObject penguinHome;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject BasketFishPosition;
    [SerializeField] private GameObject BasketFish;
    [SerializeField] private GameObject PlayerPosition;
    [SerializeField] private GameObject BasketSnowBallPosition;
    [SerializeField] private GameObject BasketSnowBall;

    [SerializeField] private AudioClip clipPenguinSounds;
    [SerializeField] private AudioClip clipHey;
    [SerializeField] private GameObject PlayerVR;


    public int sleepingTime = 10;


    public Actions _lastAction;
    private float distHungry;
    private float distSleep;
    private float distStayInFrontOfPlayer;
    private float distPlayful;
    private float distWalking;






    private float timer;
    private Transform target = null;
    public float turnSpeed = 2.0f;
    public enum Actions
    {
        Move,
        Stay, 
        Sleep,
        Hitted,
        Waving,
        TurningFromThePlayer,
        HittedFish,
        Eating,
        WaitingForFish,
        Happy
    }
    void Start()
    {
        GetComponent<penguinModel>().setupActs();

        agent = GetComponent<NavMeshAgent>();
        _origin = transform.position;
        target = Player.transform;
    }


    void Update()
    {
        
        //Игрок говорит "hey"
        if(Input.GetKeyDown(KeyCode.F))
        {
            PlayerVR.GetComponent<AudioSource>().PlayOneShot(clipHey);
            string action = GetComponent<penguinModel>().launchNewScene("HelloPinguin");
            PenguinActions(action + " HelloPinguin");
        }


        if (Input.GetKeyDown(KeyCode.N) && !isSleep && !Stop)
        {
            
            PlayerVR.GetComponent<AudioSource>().PlayOneShot(clipHey);
            Debug.Log("Hello");
            string action = GetComponent<penguinModel>().launchNewScene("HelloPinguin");
            PenguinActions(action + " HelloPinguin");
        }
            //Поворачивается к игроку и машет ему 
            if (Input.GetKeyDown(KeyCode.K) && !isSleep && !Stop)
        {
            Stop = true;
            PlayerVR.GetComponent<AudioSource>().PlayOneShot(clipHey);
            Debug.Log("Hello");
            isCoroutineExecuting = false;
            agent.SetDestination(transform.position);
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            _lastAction = Actions.Waving;
            isTurning = true;
        }
        if (Input.GetKeyDown(KeyCode.O) && !isSleep && !Stop)
        {
            Stop = true;
            isCoroutineExecuting = false;
            agent.SetDestination(transform.position);
            isStayingInFrontOfPlayer = true;
            Move(PlayerPosition.transform.position);
        }
        //Подходит к корзине с рыбой и смотрит в нее
        if (Input.GetKeyDown(KeyCode.H) && !isSleep && !Stop)
        {
            Stop = true;
            Debug.Log("Hungry");
            isCoroutineExecuting = false;
            Move(BasketFishPosition.transform.position);
            isHungry = true;

        }
        //Подходит к корзине с снежками и смотрит в нее
        if (Input.GetKeyDown(KeyCode.G) && !isSleep && !Stop)
        {
            Stop = true;
            Debug.Log("Playful");
            isCoroutineExecuting = false;
            Move(BasketSnowBallPosition.transform.position);
            isPlayful = true;

        }
        if (Input.GetKeyDown(KeyCode.U) && !isSleep && !Stop)
        {
            Stop = true;
            isCoroutineExecuting = false;
            Debug.Log("Eating");
            agent.SetDestination(transform.position);
            isCoroutineExecuting = false;
            _lastAction = Actions.Eating;
            StartCoroutine(Eating());

        }
        //Отворачивается от игрока
        if (Input.GetKeyDown(KeyCode.L) && !isSleep && !Stop)
        {
            Stop = true;
            PlayerVR.GetComponent<AudioSource>().PlayOneShot(clipHey);
            Debug.Log("Turn from the player");
            isCoroutineExecuting = false;
            agent.SetDestination(transform.position);
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            _lastAction = Actions.TurningFromThePlayer;
            isTurning = true;         
        }
        //Ждет рыбу
        if (Input.GetKeyDown(KeyCode.Y) && !isSleep && !Stop)
        {
            Stop = true;
            isCoroutineExecuting = false;
            agent.SetDestination(transform.position);
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            _lastAction = Actions.WaitingForFish;
            isTurning = true;
        }

        distPlayful = (agent.pathPending && isPlayful)
            ? Vector3.Distance(transform.position, BasketSnowBallPosition.transform.position)
            : agent.remainingDistance;
        distStayInFrontOfPlayer = (agent.pathPending && isStayingInFrontOfPlayer)
            ? Vector3.Distance(transform.position, PlayerPosition.transform.position)
            : agent.remainingDistance;
        distHungry = (agent.pathPending && isHungry)
            ? Vector3.Distance(transform.position, BasketFishPosition.transform.position)
            : agent.remainingDistance;
        distSleep = (agent.pathPending && isSleep)
            ? Vector3.Distance(transform.position, penguinHome.transform.position)
            : agent.remainingDistance;
        distWalking = (agent.pathPending && isWalking)
            ? Vector3.Distance(transform.position, _lastDestination)
            : agent.remainingDistance;


        if (distPlayful ==0 && !isCoroutineExecutingPlayful && isPlayful)
        {
            StartCoroutine(Playfullying());
        }
        if (isTurning && !isSleep)
        {
            Rotate();
        }

        if(distHungry == 0  && !isCoroutineExecutingHungry && isHungry)
        {
            StartCoroutine(Hungrying());   
        }
        if (distSleep == 0  && !isCoroutineExecutingSleep && isSleep)
        {
            StartCoroutine(Sleeping());
        }
        if (distStayInFrontOfPlayer == 0  && !isCoroutineExecutingStayingInFrontOfPlayer  &&  isStayingInFrontOfPlayer)
        {

                StartCoroutine(StayingInFrontOfPlayer());
        }
        // Проверяется , не в пути ли пингвин , либо он еще не достоял свое. 
        if (!isCoroutineExecuting && distWalking < 0.1 && !isSleep && !isHungry && !Stop)
        {
            //Choose the action (Penguin). 
            int rand = UnityEngine.Random.Range(0, 22);
            if (rand > 0 && rand < 14)
            {
                Debug.Log("Move");
                Move();
            }
            else if (rand > 14 && rand < 22)
            {
                Debug.Log("Stay");
                StartCoroutine(Stay());
            }
            else if (rand > 20 && rand < 22)
            {
                Sleep(new object());
            }
        }

    }

    private void FixedUpdate()
    {
        //Debug.Log(GetComponent<Animator>().GetBool(1));
      

    }



    public void sad() {
            Stop = true;
            Debug.Log("Sad penguin");
            isCoroutineExecuting = false;
            agent.SetDestination(transform.position);
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            _lastAction = Actions.TurningFromThePlayer;
            isTurning = true;
    }
    public void happy() {
        Stop = true;
        isCoroutineExecuting = false;
        agent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        _lastAction = Actions.Happy;
        isTurning = true;
    }

    IEnumerator HappyPenguin() {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWaitingForFish", true);
        GetComponent<AudioSource>().PlayOneShot(clipPenguinSounds);
        Debug.Log("Happy penguin");
        yield return new WaitForSeconds(2f);
        GetComponent<AudioSource>().Stop();
        GetComponent<Animator>().SetBool("isWaitingForFish", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        Stop = false;
    }
    public void Rotate()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        if (_lastAction == Actions.TurningFromThePlayer)
            targetDirection = -targetDirection;

        targetDirection.y = 0;
        // The step size is equal to speed times frame time.
        float singleStep = turnSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        if (Vector3.Angle(transform.forward, targetDirection) < 6)
        {
            isTurning = false;
            if (_lastAction == Actions.Waving)
            {
                StartCoroutine(Waving());
            }
            else if (_lastAction == Actions.Hitted)
            {
                StartCoroutine(hitReactionCoroutine());
            }
            else if (_lastAction == Actions.TurningFromThePlayer)
            {
                StartCoroutine(TurnFromThePLayer());
            }
            else if (_lastAction == Actions.HittedFish)
            {
                StartCoroutine(hitReactionFishCoroutine());
            }
            else if(_lastAction == Actions.WaitingForFish)
            {
                StartCoroutine(WaitingForFish());
            }
            else if(_lastAction == Actions.Happy)
            {
                StartCoroutine(HappyPenguin());
            }

        }
        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    IEnumerator WaitForNextFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator Eating()
    {
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        yield return new WaitForSeconds(0.2f);
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isEating", true);
        yield return new WaitForSeconds(0.3f);
        _fishEating =  Instantiate(fishEating) as GameObject;
        _fishEating.transform.position = fishPosition.transform.position;
        _fishEating.transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(1f);
        GetComponent<Animator>().SetBool("isEating", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        Destroy(_fishEating.gameObject);
        Stop = false;

    }
    IEnumerator StayingInFrontOfPlayer()
    {
        if (isCoroutineExecutingStayingInFrontOfPlayer)
            yield break;
        isCoroutineExecutingStayingInFrontOfPlayer = true;
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        yield return new WaitForSeconds(2);
        isCoroutineExecutingStayingInFrontOfPlayer = false;
        isStayingInFrontOfPlayer = false;
        Stop = false;
    }

    public void hitReactionFish()
    {
        if (!isSleep && !Stop)
        {
            
            if(!eatFish)
            {
                Stop = true;
                isCoroutineExecuting = false;
                agent.SetDestination(transform.position);
                GetComponent<Animator>().SetBool("isWalking", false);
                GetComponent<Animator>().SetBool("isStaying", true);
                _lastAction = Actions.HittedFish;
                isTurning = true;
            }
            else if(eatFish)
            {
                Stop = true;
                isCoroutineExecuting = false;
                Debug.Log("Eating");
                agent.SetDestination(transform.position);
                isCoroutineExecuting = false;
                _lastAction = Actions.Eating;
                StartCoroutine(Eating());
            }
            

        }
    }
    public void hitReaction()
    {
        if(!isSleep && !Stop)
        {
            Stop = true;
            isCoroutineExecuting = false;
            agent.SetDestination(transform.position);
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            _lastAction = Actions.Hitted;
            isTurning = true;
        }

    }


    IEnumerator hitReactionFishCoroutine()
    {
        GetComponent<Animator>().SetBool("isThrowingAside", true);
        GetComponent<Animator>().SetBool("isStaying", false);
        _fish = Instantiate(fish) as GameObject;
        Vector3 fishPosition = GameObject.Find("RightForeArm_end").transform.position;
        _fish.transform.position = fishPosition;
        _fish.transform.Rotate(40, -90, 0);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetBool("isThrowingAside", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        yield return new WaitForEndOfFrame();
        Debug.Log("Turn from the player");
        isCoroutineExecuting = false;
        _lastAction = Actions.TurningFromThePlayer;
        isTurning = true;
    }
    IEnumerator hitReactionCoroutine()
    {

        if(throwToPlayer)
        {
            GetComponent<Animator>().SetBool("isThrowingAtThePlayer", true);
            GetComponent<Animator>().SetBool("isStaying", false);
            _snowball = Instantiate(snowBall) as GameObject;
            Vector3 snowBallPosition = GameObject.Find("RightArm").transform.position;
            snowBallPosition.x -= 0.5f;
            snowBallPosition.z -= 0.5f;
            _snowball.transform.position = snowBallPosition;
            _snowball.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(0.5f);
            GetComponent<Animator>().SetBool("isThrowingAtThePlayer", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            yield return new WaitForEndOfFrame();
            Stop = false;
        }
        else if(!throwToPlayer)
        {
            GetComponent<Animator>().SetBool("isThrowingAside", true);
            GetComponent<Animator>().SetBool("isStaying", false);
            _snowball = Instantiate(snowBall) as GameObject;
            Vector3 snowBallPosition = GameObject.Find("RightForeArm_end").transform.position;
            _snowball.transform.position = snowBallPosition;
            _snowball.transform.Rotate(40, -90, 0);
            yield return new WaitForSeconds(0.5f);
            GetComponent<Animator>().SetBool("isThrowingAside", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            yield return new WaitForEndOfFrame();
            Debug.Log("Turn from the player");
            isCoroutineExecuting = false;
            _lastAction = Actions.TurningFromThePlayer;
            isTurning = true;
        }

    }

    public void WaitForFish()
    {
        Stop = true;
        isCoroutineExecuting = false;
        agent.SetDestination(transform.position);
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        _lastAction = Actions.WaitingForFish;
        isTurning = true;
        
    }

    IEnumerator reactionToFish()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("reactionToFish() " + waitingForFish);
        if (waitingForFish)
        {
            Debug.Log("Ne dojdalsya ryby");
        }
        else
        {
            Debug.Log("Polychil ryby");
        }
    }
    IEnumerator Waving()
    {
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWaving", true);
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetBool("isWaving", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        StartCoroutine(StayCurrentTime());
        yield return new WaitForSeconds(1.5f);
        //yield return new WaitForEndOfFrame();
        Stop = false;
    }
    IEnumerator TurnFromThePLayer()
    {
        Vector3 position = transform.position;
        Move(transform.position + 3*transform.forward);
        yield return new WaitForEndOfFrame();
        Stop = false;
    }

    IEnumerator WaitingForFish()
    {
        StartCoroutine(reactionToFish());
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWaitingForFish", true);
        GetComponent<AudioSource>().PlayOneShot(clipPenguinSounds);
        yield return new WaitForSeconds(4f);
        GetComponent<AudioSource>().Stop();
        GetComponent<Animator>().SetBool("isWaitingForFish", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        Stop = false;
    }



    public void Move()
    {
        _lastAction = Actions.Move;
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWalking", true);
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _maxDistance;
        randomDirection += _origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, _maxDistance, _layermask);
        _lastDestination = navHit.position;
        isWalking = true;
        agent.SetDestination(navHit.position);
    }
    IEnumerator Hungrying()
    {
        if (isCoroutineExecutingHungry)
            yield break;
        isCoroutineExecutingHungry = true;
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        transform.LookAt(BasketFish.transform);
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isLowerTheHead", true);
        GetComponent<AudioSource>().PlayOneShot(clipPenguinSounds);
        yield return new WaitForSeconds(4);
        GetComponent<AudioSource>().Stop();
        isHungry = false;
        isCoroutineExecutingHungry = false;
        GetComponent<Animator>().SetBool("isLowerTheHead", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        Stop = false;
    }

    IEnumerator Playfullying()
    {
        if (isCoroutineExecutingPlayful)
            yield break;
        isCoroutineExecutingPlayful = true;
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        transform.LookAt(BasketSnowBall.transform);
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isLowerTheHead", true);
        GetComponent<AudioSource>().PlayOneShot(clipPenguinSounds);
        yield return new WaitForSeconds(4);
        GetComponent<AudioSource>().Stop();
        isPlayful = false;
        isCoroutineExecutingPlayful = false;
        GetComponent<Animator>().SetBool("isLowerTheHead", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        yield return new WaitForEndOfFrame();
        Move(_origin);
        Stop = false;
    }
    public void Move(Vector3 pos)
    {
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWalking", true);
        NavMeshHit navHit;
        NavMesh.SamplePosition(pos, out navHit, _maxDistance, _layermask);
        _lastDestination = navHit.position;
        isWalking = true;
        agent.SetDestination(navHit.position);
    }

    IEnumerator Stay()
    {
        _lastAction = Actions.Stay;
        if (isCoroutineExecuting)
            yield break;
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        isCoroutineExecuting = true;
        //Время , которое пингвин стоит на месте. 
        yield return new WaitForSeconds(_stayTime);
        isCoroutineExecuting = false;
        GetComponent<Animator>().SetBool("isStaying", false);
    }

    IEnumerator StayCurrentTime()
    {
        _lastAction = Actions.Stay;
        if (isCoroutineExecuting)
            yield break;
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        isCoroutineExecuting = true;
        //Время , которое пингвин стоит на месте. 
        yield return new WaitForSeconds(1.5f);
        isCoroutineExecuting = false;
        GetComponent<Animator>().SetBool("isStaying", false);
    }



    public void Sleep(object num)
    {
        _lastAction = Actions.Sleep;
        Move(penguinHome.transform.position);
        isCoroutineExecuting = false;
        isSleep = true;
    }

    IEnumerator Hungry()
    {
        Move(BasketFishPosition.transform.position);
        yield return new WaitForEndOfFrame();
        isCoroutineExecutingHungry = false;
        isHungry = true;
    }

    IEnumerator Sleeping ()
    {
        if (isCoroutineExecutingSleep)
            yield break;
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isSleeping", true);
        isCoroutineExecutingSleep = true;
        yield return new WaitForSeconds(sleepingTime);
        isSleep = false;
        isCoroutineExecutingSleep = false;
        GetComponent<Animator>().SetBool("isSleeping", false);
    }

    public void GoToPlayer()
    {
        if(false)
        {
            Move(Player.GetComponent<Transform>().position);
        }
    }

    public void PenguinActions(String action)
    {
        if(action == "positive HelloPinguin")
        {
            if(!isSleep && !Stop)
            {
                Stop = true;
                //PlayerVR.GetComponent<AudioSource>().PlayOneShot(clipHey);
                Debug.Log("Hello");
                isCoroutineExecuting = false;
                agent.SetDestination(transform.position);
                GetComponent<Animator>().SetBool("isWalking", false);
                GetComponent<Animator>().SetBool("isStaying", true);
                _lastAction = Actions.Waving;
                isTurning = true;
            }
        }

        if (action == "ignore HelloPinguin")
        {
            Debug.Log("NoHello");
        }
        if (action == "goToBoxWithFish")
        {
            if(!isSleep && !Stop)
            {
                Stop = true;
                Debug.Log("Hungry");
                isCoroutineExecuting = false;
                Move(BasketFishPosition.transform.position);
                isHungry = true;
            }
        }

        if (action == "goToBoxWithSnowBalls")
        {
            if (!isSleep && !Stop)
            {
                Stop = true;
                Debug.Log("Playful");
                isCoroutineExecuting = false;
                Move(BasketSnowBallPosition.transform.position);
                isPlayful = true;
            }
        }

        if(action == "GoCommunicatePenguin")
        {
            if(!isSleep && !Stop)
            {
                Stop = true;
                isCoroutineExecuting = false;
                agent.SetDestination(transform.position);
                isStayingInFrontOfPlayer = true;
                Move(PlayerPosition.transform.position);
            }
        }
        //if(action == "eatingFish")
        //{
        //    if(!isSleep && !Stop)
        //    {
        //        Stop = true;
        //        isCoroutineExecuting = false;
        //        Debug.Log("Eating");
        //        agent.SetDestination(transform.position);
        //        isCoroutineExecuting = false;
        //        _lastAction = Actions.Eating;
        //        StartCoroutine(Eating());
        //    }
        //}

        if(action == "turnFromPlayer")
        {
            if(!isSleep && !Stop)
            {
                Stop = true;
                Debug.Log("Turn from the player");
                isCoroutineExecuting = false;
                agent.SetDestination(transform.position);
                GetComponent<Animator>().SetBool("isWalking", false);
                GetComponent<Animator>().SetBool("isStaying", true);
                _lastAction = Actions.TurningFromThePlayer;
                isTurning = true;
            }
        }

        if(action == "turnToPlayerAndMashetKrylyamijdetryby")
        {
            if(!isSleep && !Stop)
            {
                Stop = true;
                isCoroutineExecuting = false;
                agent.SetDestination(transform.position);
                GetComponent<Animator>().SetBool("isWalking", false);
                GetComponent<Animator>().SetBool("isStaying", true);
                _lastAction = Actions.WaitingForFish;
                isTurning = true;
            }
        }
        if(action == "positive ThrowBallToPinguin")
        {
            if(!isSleep && !Stop)
            {
                throwToPlayer = true;
                hitReaction();
            }
        }
        if (action == "ignore ThrowBallToPinguin")
        {
            if (!isSleep && !Stop)
            {
                throwToPlayer = false;
                hitReaction();
            }
        }
        if(action == "ignore FeedPinguin")
        {
            if(!isSleep && !Stop)
            {
                eatFish = false;
                hitReactionFish();
            }
        }
        if (action == "positive FeedPinguin")
        {
            if (!isSleep && !Stop)
            {
                eatFish = true;
                hitReactionFish();
            }
        }
        if (action == "positive StrokePinguin")
        {
            if(!isSleep && !Stop)
            {
                happy();
            }
        }
        if (action == "ignore StrokePinguin")
        {
            if (!isSleep && !Stop)
            {
                PenguinActions("turnFromPlayer");
            }
        }
        if (action == "SadPenguinAfterHit")
        {
            if (!isSleep && !Stop)
            {
                sad();
            }
        }

    }


}
