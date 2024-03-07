using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
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
        Attack,      
    }

    /*-------------------------------------------*   VALUES   *--------------------------------*/
    [Header("Behavior Relationships")]
    [SerializeField] public NPC_Behavior_Sight sightBehavior;

    [Header("Patrol Behavior Settings")]
    public NPCState currentState = NPCState.Idle;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public int patrolNodeIndex;
    [SerializeField] List<GameObject> PatrolNodes;
    [SerializeField] public Queue<GameObject> enemyNavQueue = new Queue<GameObject>();
    [SerializeField] public int numofPatrolNodes;
    [SerializeField] public float distance;
    [SerializeField] public Vector3 targetDestination;
    [SerializeField] public float counter;
    [SerializeField] public int setTimer;
    [SerializeField] public bool threatSpotted;
    private Transform getPlayerPosition;
    private bool isNavigatingTowardsPlayer = false;
    private Vector3 targetPosition;
    private void Awake()
    {
        sightBehavior = GetComponent<NPC_Behavior_Sight>();
        PlanRoute();
        agent = GetComponent<NavMeshAgent>();

        PatrolNodes[0].transform.parent.SetParent(null);
        //PatrolNodes[0].transform.parent.gameObject.hideFlags = HideFlags.HideInHierarchy;
        
    }
    void Update()
    {
        
        seenAnomaly();
        if (patrolNodeIndex == numofPatrolNodes)
        {
            PatrolNodes.Reverse();
            patrolNodeIndex = 0;
        }


        setTargetPosition();
          /*---------------------------*   States   *-----------------------------------------------------------------*/
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
            case NPCState.Attack:
                AttackState();
                
                break;
        }
    }
     /* -------------------------------------*   State Behaviors   *--------------------------------------------*/
    void IdleState()
    {
        goTowards();
        if (counter >= setTimer)
        {
            
            currentState = NPCState.Patrol;
        }  
       
        counter = counter + Time.deltaTime;
    }
    void PatrolState()
    {
        counter = 0;
        if (enemyNavQueue.Count != 0) // if there are waypoints in queue
        {
            targetDestination = enemyNavQueue.Dequeue().transform.position;           
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
        //Debug.Log("investigation state");
        counter = 0;
        //Vector3 npcPOS = agent.transform.position;
            enemyNavQueue.Clear(); //clear queue 
        if (enemyNavQueue.Count <= 0 && !isNavigatingTowardsPlayer) // if there are no waypoints in queue
        {
            enemyNavQueue.Enqueue(sightBehavior.playerReference);//place player seen location in queue
            
           // Debug.Log("placed waypoint in queue");
            if (enemyNavQueue.Count != 0) //if there are waypoints in queue
            {
                isNavigatingTowardsPlayer = true;
                setPlayerPosition(enemyNavQueue.Dequeue().transform);

                
                targetDestination = GetPlayerPosition.position;

                agent.SetDestination(targetDestination);
               // Debug.Log("Moving to target");

               

            }

        }
        if (distance <= 1f)
        {
           // Debug.Log("Distance Met");
            isNavigatingTowardsPlayer = false;
            currentState = NPCState.Idle;
        }
    }
   void AttackState()
    {
        while(sightBehavior.canSeePlayer){
            
            if (distance <= 15f)
            {

            }
        }
    }
    /*------------------------------------------------------------*   Functions   *---------------------------------------------------------------*/

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
    }

    private void setTargetPosition() 
    {
        if (currentState == NPCState.Patrol || currentState == NPCState.Idle)
        {
            distance = Vector3.Distance(agent.transform.position, PatrolNodes[patrolNodeIndex].transform.localPosition); 
        }
        if(currentState == NPCState.Investigate)
        {
            distance = Vector3.Distance(agent.transform.position, targetDestination);
        }
       
    }

    public void PlanRoute()
         {
        // store waypoints into an array
        foreach(Transform child in transform.GetChild(0))
        {
            if( child.CompareTag("PatrolNode"))
            {
                PatrolNodes.Add(child.gameObject);
            }
        }

        //GameObject[] findNodes = GameObject.FindGameObjectsWithTag("PatrolNode");
        //     // loop each time a waypoint is found in an array then store the waypoint into a list.
        //     foreach (GameObject patrolNode in findNodes)
        //        {       
        //              PatrolNodes.Add(patrolNode);  
        //              numofPatrolNodes++;
        //        }                 
         }
     public void goTowards()
    {
        if (enemyNavQueue.Count == 0)//if queue is empty
        {
            enemyNavQueue.Enqueue(PatrolNodes[patrolNodeIndex]);//add location from list at index x into queue, 
            Debug.Log(enemyNavQueue.Count);
            /* if the index is equal to the patrol nodes list reverse the patrol nodes list then reset the index count*/   
        }
    }
    public void seenAnomaly()
    {
      if (sightBehavior.canSeePlayer && currentState != NPCState.Investigate)
        {
            Debug.Log("player seen");
            currentState = NPCState.Investigate;
        }
    }

    private void ResetPatrolTimer()
    {
        counter = 0;
    }

    public void ThreatSpotted()
    {
    
        while (sightBehavior.canSeePlayer)
        {

            if (distance <=  5f)
            {

            }

           
        }
    }


    /*------------------------------------------*   getters & setters   *-----------------------------------------*/
    /*
    public Queue<Transform> PatrolPointsQueue
    {
        get
        {
            return PatrolPointsQueue;
        }
    }
    */
    public Transform GetPlayerPosition
    {
        get { return getPlayerPosition; }
    }
    public void setPlayerPosition(Transform newPlayerPosition)
    {
        if (getPlayerPosition == null)
        {
            getPlayerPosition = newPlayerPosition;
        }
    }


    
}
