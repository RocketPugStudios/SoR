using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        if (behaviorStateMachine.currentState == NPC_behavior_StateMachine.NPCState.Patrol)
        {
            animator.SetBool("isWalking", true);
        }
    }

/*
    private void OnEnable()
    {
        Actions.walking += startWalkAnimation;
        Actions.stopWalking += stopWalkAnimation;
    }

    private void OnDisable()
    {
        Actions.walking -= startWalkAnimation;
        Actions.stopWalking -= stopWalkAnimation;
    }

    private void startWalkAnimation()
    {
       this.animator.SetBool("isWalking",true);
       
    }
    private void stopWalkAnimation()
    {
        this.animator.SetBool("isWalking", false);

    }

    */
}

