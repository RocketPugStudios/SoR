using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Behavior_LightSoldier : MonoBehaviour
{
    public NavMeshAgent agent { get; private set;}
    public int patrolNodeElement;
    [SerializeField] List<GameObject> PatrolNodes;
    public int numofPatrolNodes;
    public float distance;
    public float nxtPositionDelay;
    public float countDown;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        getPatrolNodes();      
    }
    private void Update()
    {
        agent.SetDestination(PatrolNodes[patrolNodeElement].transform.localPosition);
        setNextDestination();
    }
    private void getPatrolNodes()
    {
        GameObject[] findNodes = GameObject.FindGameObjectsWithTag("PatrolNode");
        foreach (GameObject patrolNode in findNodes)
        {
            PatrolNodes.Add(patrolNode);
            numofPatrolNodes++;
        }  
    }
    private void setNextDestination()
    {
        
        
        distance = Vector3.Distance(agent.transform.position, PatrolNodes[patrolNodeElement].transform.localPosition);
        if (distance <= 2f)
        {
            countDown = countDown - Time.deltaTime;
            if (countDown <= 0f)
            {
                countDown = countDown - Time.deltaTime;
                patrolNodeElement++;
                Debug.Log("NextNode");

                if (patrolNodeElement == numofPatrolNodes)
                {

                    patrolNodeElement = 0;
                }
            }
           
        }  
    }
}
