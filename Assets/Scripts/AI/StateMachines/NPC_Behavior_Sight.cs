using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NPC_Behavior_Sight : MonoBehaviour
{
    [Header("Relationships")]
    [SerializeField] public GameObject playerReference;

    [Header("Anom. Sight Settings")]
    [SerializeField] public float sphere_radius;
    [Range(0, 360)]
    [SerializeField] public float angle;
    [SerializeField] public bool canSeePlayer;

    [Header("Layer Maskes")]
    [SerializeField] public LayerMask Player;
    [SerializeField] public LayerMask Obstacle;

    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, sphere_radius, Player);

        //Check if there any colliders available
        if (rangeChecks.Length != 0)
        {
          //  Debug.Log("First Check Complete");
            // Get the first target's transform
            Transform target = rangeChecks[0].transform;

            // Calculate the normalized direction from current position to target
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Check if the angle between forward direction and direction to target is within the FOV
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
               // Debug.Log("Second Check Complete");
                // Calculate the distance to the target
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // Raycast to check for obstacles in the line of sight
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, Obstacle))
                {
                    canSeePlayer = true; // Player is within FOV and not obstructed by obstacles
                   // Debug.Log("player seen");
                }
                else
                    canSeePlayer = false; // Player is obstructed by obstacles
            }
            else
                canSeePlayer = false; // Player is outside of FOV
        }
        else if (canSeePlayer)
            canSeePlayer = false; // No targets found, reset canSeePlayer flag
    }
   
   
    
}
