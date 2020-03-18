using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class clickToMove : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Camera _camera;
    private penguinAI _ai;


    public GameObject penguinHome;
    private void Start()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //_ai = GetComponent<penguinAI>();
            //_ai.enabled = false;
            RaycastHit hit;
            if(Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition) , out hit))
            {
                GetComponent<penguinAI>().Move(hit.point);
            }
        }
    }
}
