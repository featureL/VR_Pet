using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class penguinAI : MonoBehaviour
{
    private NavMeshAgent agent;

    
    private int _layermask = -1;
    public float _maxDistance = 25;
    private Vector3 _origin;
    private bool isCoroutineExecuting = false;


    private Actions _lastAction;


    private enum Actions
    {
        Move,
        Stay
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _origin = transform.position;
        Move();
    }

    
    void Update()
    {
        // Проверяется , не в пути ли пингвин , либо он еще не достоял свое. 
        if (!isCoroutineExecuting && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            //Выбирается действие для пингвина. 
            int rand = Random.Range(0, 8);
            if(rand > 2) { Debug.Log("Move"); Move(); }
            else { Debug.Log("Stay"); StartCoroutine(Stay());}
            
        }
            
    }


    public void Move()
    {
        //Debug.Log(agent.remainingDistance);
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _maxDistance;
        //Debug.Log(UnityEngine.Random.insideUnitSphere);
        randomDirection += _origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, _maxDistance, _layermask);
        agent.SetDestination(navHit.position);
        return;
    }

    IEnumerator Stay()
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;
        //Время , которое пингвин стоит на месте. 
        yield return new WaitForSeconds(5);
        isCoroutineExecuting = false;
    }
}
