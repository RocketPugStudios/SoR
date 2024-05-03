using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_animation_stateMachine : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] public Animation _animation;
    [SerializeField] public Animator animator;
    [SerializeField] public NPC_behavior_StateMachine behaviorStateMachine;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        behaviorStateMachine = this.GetComponent<NPC_behavior_StateMachine>();
    }

    private void Update()
    {
        if ( behaviorStateMachine.currentState == NPC_behavior_StateMachine.NPCState.Idle)
        {
            animator.SetBool("isWalking",false);
            animator.SetBool("playerisSpotted", false);
        }

        if (behaviorStateMachine.currentState == NPC_behavior_StateMachine.NPCState.Patrol)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("playerisSpotted", false);
        }

        if (behaviorStateMachine.currentState == NPC_behavior_StateMachine.NPCState.Investigate)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("playerisSpotted", false);
        }

        if (behaviorStateMachine.currentState == NPC_behavior_StateMachine.NPCState.Combat)
        {
            if (!this.behaviorStateMachine.isInRangeOfPlayer && this.behaviorStateMachine.isGettingInRange)
            {
                animator.SetBool("isChasingPlayer" , true);
                this.GetComponent<NavMeshAgent>().speed = 4.5f;
            }
            if (this.behaviorStateMachine.isInRangeOfPlayer && !this.behaviorStateMachine.isGettingInRange)
            {
                animator.SetBool("playerisSpotted", true);
                animator.SetBool("isChasingPlayer", false);
            }
          // else
        
        }
    }
}

