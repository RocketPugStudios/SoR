using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NPC_behavior_StateMachine : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Patrol,
        Investigate,
        
    }
        public NPCState currentState = NPCState.Idle;
        // Patrol behavior settings 
        public NavMeshAgent agent;
        public int patrolNodeIndex;
        [SerializeField] List<GameObject> PatrolNodes;
        public Queue<GameObject> playerNavQueue = new Queue<GameObject>();
        public int numofPatrolNodes;
        public float distance;
        public bool routeLoaded;
        public Vector3 targetDestination;
        public bool moving;
        public float countdown;
    private void Awake()
    {
        PlanRoute();
        agent = GetComponent<NavMeshAgent>();  
    }
    void Update()
    {
        if (patrolNodeIndex == numofPatrolNodes)
        {
            PatrolNodes.Reverse();
            patrolNodeIndex = 0;
        }
        distance = Vector3.Distance(agent.transform.position, PatrolNodes[patrolNodeIndex].transform.localPosition);

        switch (currentState)
        {
            case NPCState.Idle:
                IdleState();
                break;
            case NPCState.Patrol:
                PatrolState();
                break;
            case NPCState.Investigate:
                InvestigationState();
                break;
                
        }
    }

    void IdleState()
    {
        goTowards();
        countdown = countdown - Time.deltaTime ;
        if (countdown <= 0f)
        {
            currentState = NPCState.Patrol;
        }
    }
    void PatrolState()
    {
        countdown = 0;
        if (playerNavQueue.Count != 0)
        {
            targetDestination = playerNavQueue.Dequeue().transform.localPosition;           
            agent.SetDestination(targetDestination);
        }
       if(distance <= 2f)
        {        
            patrolNodeIndex++;
            currentState = NPCState.Idle;
        }
    }
    void InvestigationState()
    {

    }
    public void PlanRoute()
         {
             // store waypoints into an array
             GameObject[] findNodes = GameObject.FindGameObjectsWithTag("PatrolNode");

             // loop each time a waypoint is found in an array then store the waypoint into a list.
             foreach (GameObject patrolNode in findNodes)
                {       
                      PatrolNodes.Add(patrolNode);  
                      numofPatrolNodes++;
                }                 
         }
     public void goTowards()
    {
        if (playerNavQueue.Count == 0 && !moving)//if queue is empty
        {
            playerNavQueue.Enqueue(PatrolNodes[patrolNodeIndex]);//add location from list at index x into qeueu, 
            Debug.Log(playerNavQueue.Count);
            /* if the index is equal to the patrol nodes list reverse the patrol nodes list then reset the index count*/   
        }
    }


    public void playerPosition()
    {
        Transform currentplayerposition;
        if (this.gameObject.GetComponent<NPC_Behavior_Sight>().canSeePlayer)
        {
            currentplayerposition = this.gameObject.GetComponent<NPC_Behavior_Sight>().playerReference.transform;
            PatrolPointsQueue.Enqueue(currentplayerposition) ;
        }
    }
    //getters & setters
    public Queue<Transform> PatrolPointsQueue
    {
        get
        {
            return PatrolPointsQueue;
        }
    }
}
