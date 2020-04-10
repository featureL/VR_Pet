using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class penguinAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private GameObject snowBall;
    private GameObject _snowball;
    [SerializeField] private GameObject fish;
    private GameObject _fish;


    private int _layermask = -1;
    public float _maxDistance = 12;
    
    private Vector3 _origin;
    
    private bool isCoroutineExecuting = false;
    private bool isCoroutineExecutingSleep = false;
    private bool isCoroutineExecutingHungry = false;
    private bool isCoroutineExecutingStayingInFrontOfPlayer = false;
    private bool isCoroutineExecutingPlayful = false;
    private bool isSleep = false;
    public bool Stop = false;
    private bool isHungry = false;
    private bool isPlayful = false;
    private bool isTurning = false;
    private bool isStayingInFrontOfPlayer = false;

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







    private float timer;
    private Transform target = null;
    public float turnSpeed = 1.0f;
    public enum Actions
    {
        Move,
        Stay, 
        Sleep,
        Hitted,
        Waving,
        TurningFromThePlayer,
        HittedFish
    }
    void Start()
    {
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
        
        if(distPlayful ==0 && !isCoroutineExecutingPlayful && isPlayful)
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

        if (!isCoroutineExecuting && agent.remainingDistance == 0 && !isSleep && !isHungry && !Stop)
        {
            //Choose the action (Penguin). 
            int rand = Random.Range(0, 22);
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

        Debug.Log(Vector3.Angle(transform.forward, targetDirection));
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

        }
        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    IEnumerator WaitForNextFrame()
    {
        yield return new WaitForEndOfFrame();
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
            Stop = true;
            Debug.Log("Snow hit");
            isCoroutineExecuting = false;
            agent.SetDestination(transform.position);
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isStaying", true);
            _lastAction = Actions.HittedFish;
            isTurning = true;
        }
    }
    public void hitReaction()
    {
        if(!isSleep && !Stop)
        {
            Stop = true;
            Debug.Log("Snow hit");
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
        int randomNum = Random.Range(0, 2);

        if(randomNum == 0)
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
        else if(randomNum == 1)
        {
            Debug.Log("RandomNum");
            Debug.Log(randomNum);
            GetComponent<Animator>().SetBool("isThrowingAside", true);
            GetComponent<Animator>().SetBool("isStaying", false);
            _snowball = Instantiate(snowBall) as GameObject;
            Vector3 snowBallPosition = GameObject.Find("RightForeArm_end").transform.position;
            //snowBallPosition.x -= 0.5f;
            //snowBallPosition.z -= 0.5f;
            //Vector3 snowBallRotation = new Vector3(0, 90, 45);
            //_snowball.transform.eulerAngles = snowBallRotation;
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



    public void Move()
    {
        _lastAction = Actions.Move;
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWalking", true);
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _maxDistance;
        randomDirection += _origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, _maxDistance, _layermask);
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


}
