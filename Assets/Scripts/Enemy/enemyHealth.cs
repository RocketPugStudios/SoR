using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
   [SerializeField] public int health = 3;
    private bool isEnemyDead = false;
    public void OnHitByPlayer()
    {
        health--;
        GetComponent<NPC_behavior_StateMachine>().ReactToHit();
        if (health <= 0)
        {
            isEnemyDead = true;
            Destroy(gameObject);
        }
    }    
}
