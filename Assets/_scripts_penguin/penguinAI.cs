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


    private int _layermask = -1;
    public float _maxDistance = 12;
    
    private Vector3 _origin;
    
    private bool isCoroutineExecuting = false;
    private bool isCoroutineExecutingSleep = false;
    private bool isCoroutineExecutingHungry = false;
    private bool isCoroutineExecutingStayingInFrontOfPlayer = false;
    private bool isSleep = false;
    public bool Stop = false;
    private bool isHungry = false;
    private bool isTurning = false;
    private bool isStayingInFrontOfPlayer = false;

    private Vector3 _lastDestination;

    private float _stayTime = 5.0f;

    public GameObject penguinHome;
    public GameObject Player;
    public GameObject BasketFishPosition;
    public GameObject BasketFish;
    public GameObject PlayerPosition;


    public int sleepingTime = 10;


    public Actions _lastAction;
    private float distHungry;
    private float distSleep;
    private float distStayInFrontOfPlayer;







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
        TurningFromThePlayer
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _origin = transform.position;
        target = Player.transform;
    }

    
    void Update()
    {

        //Поворачивается к игроку и машет ему 
        if (Input.GetKeyDown(KeyCode.K) && !isSleep && !Stop)
        {
            Stop = true;
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
        //Отворачивается от игрока
        if (Input.GetKeyDown(KeyCode.L) && !isSleep && !Stop)
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

        distStayInFrontOfPlayer = (agent.pathPending && isStayingInFrontOfPlayer)
            ? Vector3.Distance(transform.position, PlayerPosition.transform.position)
            : agent.remainingDistance;
        distHungry = (agent.pathPending && isHungry)
            ? Vector3.Distance(transform.position, BasketFishPosition.transform.position)
            : agent.remainingDistance;
        distSleep = (agent.pathPending && isSleep)
            ? Vector3.Distance(transform.position, penguinHome.transform.position)
            : agent.remainingDistance;

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
            if (rand > 23 && rand < 22)
            {
                Debug.Log("Move");
                Move();
            }
            else if (rand > 0 && rand < 22)
            {
                Debug.Log("Stay");
                StartCoroutine(Stay());
            }
            else if (rand > 22 && rand < 22)
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
    IEnumerator hitReactionCoroutine()
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
        Stop = false;
    }

    IEnumerator Waving()
    {
        GetComponent<Animator>().SetBool("isStaying", false);
        GetComponent<Animator>().SetBool("isWaving", true);
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetBool("isWaving", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        yield return new WaitForEndOfFrame();
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
        yield return new WaitForSeconds(4);
        isHungry = false;
        isCoroutineExecutingHungry = false;
        GetComponent<Animator>().SetBool("isLowerTheHead", false);
        GetComponent<Animator>().SetBool("isStaying", true);
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
