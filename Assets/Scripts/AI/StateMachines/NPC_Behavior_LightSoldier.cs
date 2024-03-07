using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Behavior_LightSoldier : MonoBehaviour
{
    public NavMeshAgent agent {get;private set;}
    public int patrolNodeIndex;
    [SerializeField] List<GameObject> PatrolNodes;
    public Queue<Transform> PatrolNodesQueue;
    public int numofPatrolNodes;
    public float distance;
    public bool routeLoaded;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        getPatrolNodes();      
    }
    private void FixedUpdate()
    {
      
       
        //agent.SetDestination(PatrolNodes[patrolNodeIndex].transform.localPosition);
        // setNextDestination();
    }
    private void getPatrolNodes()
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
    
    private void setNextDestination()
    {
        //distance = Vector3.Distance(agent.transform.position, PatrolNodes[patrolNodeIndex].transform.localPosition);
        // When NPC reaches waypoint.
        if (distance <= 2f)
        {   
          //go to next waypoint in list  
          patrolNodeIndex++;
          Debug.Log("go to next node");
           //when NPC reaches all waypoints in list, go back in reverse order.
          if (patrolNodeIndex == numofPatrolNodes)
          {
                // reverse all nodes within list
                    PatrolNodes.Reverse();
                    patrolNodeIndex = 0;
          }
        }     
    }
    
    
  
}
