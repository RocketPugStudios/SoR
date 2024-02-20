using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_behavior_StateMachine : MonoBehaviour
{
    public enum CharacterState
    {
        IdlePatrol,
        ActivePatrol,
        
    }

    // Define triggers if your state transitions depend on specific events
    public enum Trigger
    {
        StartRunning,
        StopRunning,
        
    }

}
