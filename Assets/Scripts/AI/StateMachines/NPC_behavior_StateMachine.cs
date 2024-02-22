using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class NPC_behavior_StateMachine : MonoBehaviour
{


    public enum NPCState
    {
        Idle,
        Patrol
    }
    public NPCState currentState = NPCState.Idle;

        // Patrol behavior settings
        public Transform[] patrolPoints;
        private int currentPatrolPointIndex;
        public NavMeshAgent agent { get; private set; }
        public int patrolNodeIndex;
        [SerializeField] List<GameObject> PatrolNodes;
        public Queue<Transform> PatrolNodesQueue;
        public int numofPatrolNodes;
        public float distance;
        public bool routeLoaded;

    void Update()
        {
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
           
        }

    void PatrolState()
    {

    
        bool StartPatrol()
        {
            PlanRoute();
            
            return true;
            
        }

        bool StopPatrolling()
        {
            if(PatrolNodesQueue.Count == 0)
            {
                currentState = NPCState.Idle;
            }
            return false;
        }

        void MoveTowards(Vector3 targetPosition)
        {
            agent.SetDestination(targetPosition);
        }

        void PlanRoute()
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
    }
}
