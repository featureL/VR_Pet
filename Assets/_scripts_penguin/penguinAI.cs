using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class penguinAI : MonoBehaviour
{
    private NavMeshAgent agent;

    
    private int _layermask = -1;
    public float _maxDistance = 12;
    private Vector3 _origin;
    private bool isCoroutineExecuting = false;
    private bool isCoroutineExecutingSleep = false;

    public GameObject penguinHome;
    private bool isSleep = false;

    public int sleepingTime = 10;

    private Actions _lastAction;


    private enum Actions
    {
        Move,
        Stay, 
        Sleep
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _origin = transform.position;
        //Move(_origin+new Vector3(1,1,1));
        //TimerCallback tm = new TimerCallback(Sleep);
        //// создаем таймер
        //Timer timer = new Timer(tm, 10, 0, 10000);
        Sleep(new object());
    }

    
    void Update()
    {
        Debug.Log(isCoroutineExecuting);
        Debug.Log(agent.pathStatus);
        Debug.Log(agent.remainingDistance);
        // Проверяется , не в пути ли пингвин , либо он еще не достоял свое. 
        //&& agent.pathStatus == NavMeshPathStatus.PathComplete
        if(agent.remainingDistance == 0 && isSleep == true && isCoroutineExecutingSleep == false)
        {
            StartCoroutine(Sleeping());
        }
            
        if (!isCoroutineExecuting && agent.remainingDistance == 0 && isSleep == false)
        {
            //Выбирается действие для пингвина. 
            int rand = Random.Range(0, 15);
            if(rand > 0 && rand < 9) { 
                Debug.Log("Move"); 
                Move(); 
            }
            else if(rand >9 && rand < 15) { 
                Debug.Log("Stay"); 
                StartCoroutine(Stay());
            }
            
        }
            
    }


    public void Move()
    {
        GetComponent<Animator>().SetBool("isWalking", true);
        Debug.Log(agent.remainingDistance);
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _maxDistance;
        randomDirection += _origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, _maxDistance, _layermask);
        agent.SetDestination(navHit.position);
        return;
    }

    public void Move(Vector3 pos)
    {
        GetComponent<Animator>().SetBool("isWalking", true);
        Debug.Log(agent.remainingDistance);
        NavMeshHit navHit;
        NavMesh.SamplePosition(pos, out navHit, _maxDistance, _layermask);
        agent.SetDestination(navHit.position);
        return;
    }

    IEnumerator Stay()
    {
        if (isCoroutineExecuting)
            yield break;
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isStaying", true);
        isCoroutineExecuting = true;
        //Время , которое пингвин стоит на месте. 
        yield return new WaitForSeconds(5);
        isCoroutineExecuting = false;
        GetComponent<Animator>().SetBool("isStaying", false);
    }

    public void Sleep(object num)
    {
        Move(penguinHome.transform.position);
        isCoroutineExecuting = false;
        isSleep = true;
    }

    IEnumerator Sleeping ()
    {
        if (isCoroutineExecutingSleep)
            yield break;
        GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Animator>().SetBool("isSleeping", true);
        isCoroutineExecutingSleep = true;
        yield return new WaitForSeconds(sleepingTime);
        isSleep = false;
        isCoroutineExecutingSleep = false;
        GetComponent<Animator>().SetBool("isSleeping", false);
    }


}
