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
        if (health <= 0)
        {
            isEnemyDead = true;
            Destroy(gameObject);
        }
    }    
}
