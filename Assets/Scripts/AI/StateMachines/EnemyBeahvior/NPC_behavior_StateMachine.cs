using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEditor.UIElements;
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
        Combat,      
    }

    /*-------------------------------------------*   VALUES   *--------------------------------*/
    [Header("Relationships")]
    [SerializeField] public NPC_Behavior_Sight sightBehavior;
    [SerializeField] public enemyWeapon weaponScript;
    [SerializeField] public GameObject Player;
    public GameObject wayPoint;
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
    [Header("Distance Settings")]
    [SerializeField] public bool isInRangeOfPlayer;
    [SerializeField] public float DistanceBetweenPlayerAndGameObject;
  


    private Transform getPlayerPosition;
    private bool isNavigatingTowardsPlayer = false;
    private Vector3 targetPosition;
    private float shotTimer = 0.5f;
    private Coroutine coroutine;

    private void Awake()
    {
        sightBehavior = GetComponent<NPC_Behavior_Sight>();
        SpawnStartingPatrolPoint();
        PlanRoute();
        agent = GetComponent<NavMeshAgent>();
        PatrolNodes[0].transform.parent.SetParent(null);
        weaponScript = GetComponentInChildren<enemyWeapon>();
        //PatrolNodes[0].transform.parent.gameObject.hideFlags = HideFlags.HideInHierarchy;     
    }
    void Update()
    {
        DistanceToPlayer();
        seenAnomaly();
        CircleBackToStartPatrolNode();
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
            case NPCState.Combat:
                CombatState();     
                break;
        }
    }
     /* -------------------------------------*   State Behaviors   *--------------------------------------------*/
    void IdleState()
    {
       //Actions.stopWalking();
        goTowards();
        if (counter >= setTimer)
        {  
            currentState = NPCState.Patrol;
        }  
        counter = counter + Time.deltaTime;
    }
    void PatrolState()
    {
        Invoke("ResetPatrolTimer", 1f);
        
        if (enemyNavQueue.Count != 0) // if there are waypoints in queue
        {
            targetDestination = enemyNavQueue.Dequeue().transform.position;           
            agent.SetDestination(targetDestination);
        }
       if(distance <= 2f)
        {
            //Invoke("DelayedAction",2f);
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
            }
        }
        if (distance <= 1f)
        {
           // Debug.Log("Distance Met");
            isNavigatingTowardsPlayer = false;
            currentState = NPCState.Idle;
        }

        if (distance <= 10f && sightBehavior.canSeePlayer)
        {
            isNavigatingTowardsPlayer = false;
            StopAgent();
            
            currentState = NPCState.Combat;
        }
    }
    void CombatState()
    {     
        bool _canseeplayer = sightBehavior.canSeePlayer;
        Vector3 direction = sightBehavior.playerReference.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction); 
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        GetInRange();

        if (_canseeplayer && isInRangeOfPlayer)
        {
            if (coroutine == null) { coroutine = StartCoroutine(shootPlayer());}
        }
    }
    /*------------------------------------------------------------*   Methods  *---------------------------------------------------------------*/
    public void ReactToHit(){gameObject.transform.LookAt(Player.transform.position);}
    private void GetInRange()
    {
        if (DistanceBetweenPlayerAndGameObject >= 6.5f)
        {
            isInRangeOfPlayer = false;
            agent.SetDestination(Player.transform.position);
        }
        if (DistanceBetweenPlayerAndGameObject <= 6.5f)
        {
            StopAgent();
            isInRangeOfPlayer = true;
        }
    }
    private void CircleBackToStartPatrolNode()
    {
        if (patrolNodeIndex == numofPatrolNodes)
        {
            PatrolNodes.Reverse();
            patrolNodeIndex = 0;
        }
    }
    private void DistanceToPlayer()
    {
        float distance = Vector3.Distance(Player.transform.position, agent.transform.position);

        DistanceBetweenPlayerAndGameObject = distance;
        
    }
    private void SpawnStartingPatrolPoint()
    {
        Transform patrolNodes = transform.Find("Patrol Nodes");

        if (patrolNodes == null)
        {
            Debug.LogError("Patrol Nodes not found!");
            return;
        }

        // Instantiate the nodePrefab as a child of patrolNodes
        GameObject newNode = Instantiate(wayPoint, patrolNodes);
        String tag = "PatrolNode";
        // Optionally set the local position of the new node
        newNode.tag = tag; // set tag for gameobject
        newNode.transform.localPosition = Vector3.zero; // Position at the center of the parent
        newNode.transform.SetAsFirstSibling();
    }

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
    }

    private void setTargetPosition() 
    {
        if (currentState == NPCState.Patrol || currentState == NPCState.Idle)
        {
            distance = Vector3.Distance(this.transform.position, PatrolNodes[patrolNodeIndex].transform.position); 
        }
        if(currentState == NPCState.Investigate)
        {
            distance = Vector3.Distance(agent.transform.position, targetDestination);
        }
       
    }

    private void PlanRoute()
    {
        // store waypoints into an array
        foreach(Transform child in transform.GetChild(0))
        {
            if( child.CompareTag("PatrolNode"))
            {
                PatrolNodes.Add(child.gameObject);
                numofPatrolNodes++;
            }
        }
                
    }
     private void goTowards()
    {
        if (enemyNavQueue.Count == 0)//if queue is empty
        {
            enemyNavQueue.Enqueue(PatrolNodes[patrolNodeIndex]);//add location from list at index x into queue, 
            
        }
    }

    private void StopAgent()
    {   
        if (enemyNavQueue.Count != 0 || enemyNavQueue.Count == 0 )//if queue is empty or filled
        {
            GameObject stoppedLocation = agent.gameObject;
            enemyNavQueue.Clear();
            enemyNavQueue.Enqueue(stoppedLocation);//add location from list at index x into queue,     
            targetDestination = enemyNavQueue.Dequeue().transform.position;
            agent.SetDestination(targetDestination);
        }
    }
    public void seenAnomaly()
    {
      if (sightBehavior.canSeePlayer && currentState != NPCState.Investigate)
        {
            Debug.Log("player seen");
            if (currentState == NPCState.Combat) {return;}
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
  
    IEnumerator shootPlayer()
    {
        yield return new WaitForSeconds(1f);

        int rounds = 5;
        for ( int i = 0; i <= rounds ; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log("bang");   
            weaponScript.shootWeapon();

            if (weaponScript == null)
            {
                 yield return new WaitForSeconds(1f);              
            }
             
        }
        coroutine = null; 
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
    /*-----------------------------------------*  Subscriptions  *--------------------------------------------------*/

   





}
