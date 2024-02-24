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
        Patrol
    }
    public NPCState currentState = NPCState.Idle;

    // Patrol behavior settings

    public NavMeshAgent agent;
        public int patrolNodeIndex;
        [SerializeField] List<GameObject> PatrolNodes;
        public Queue<GameObject> PatrolNodesQueue = new Queue<GameObject>();
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
        if (PatrolNodesQueue.Count != 0)
        {
            
            targetDestination = PatrolNodesQueue.Dequeue().transform.localPosition;
            
            agent.SetDestination(targetDestination);
        }
       if(distance <= 2f)
        {
            
            patrolNodeIndex++;
            currentState = NPCState.Idle;
        }

      
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
                  //Debug.Log(PatrolNodesQueue.Count);
         }
     public void goTowards()
    {
        if (PatrolNodesQueue.Count == 0 && !moving)//if queue is empty
        {
            PatrolNodesQueue.Enqueue(PatrolNodes[patrolNodeIndex]);//add location from list at index x into qeueu, 
            //patrolNodeIndex++;//increase index x 
            Debug.Log(PatrolNodesQueue.Count);
            /* if the index is equal to the patrol nodes list reverse the patrol nodes list then reset the index count*/
            
        }
        /*
        if (patrolNodeIndex == numofPatrolNodes)
        {
            PatrolNodes.Reverse();
            patrolNodeIndex = 0;
        }
        */
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
