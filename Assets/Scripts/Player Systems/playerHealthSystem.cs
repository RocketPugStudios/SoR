using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealthSystem : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] Animator animations;
    [SerializeField] bool isPlayerDead =false;



    void Awake()
    {
        animations = GetComponent<Animator>(); 
    }
    public void playerConstitution()
    {
        health--;
        playerDeathAnimation();
    }

    public void playerDeathAnimation()
    {
        if (health <= 0)
        {
            animations.SetBool("isDead" ,true);
            bool isPlayerDead = true;
        }
        
    }
}
