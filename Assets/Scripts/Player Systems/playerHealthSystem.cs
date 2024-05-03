using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerHealthSystem : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] Animator animations;
    [SerializeField] bool isPlayerDead =false;
    [SerializeField] Coroutine coroutine;
    [SerializeField] public float deathTimer;



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
            isPlayerDead = true;
            if (coroutine == null) { coroutine = StartCoroutine(Restart());}
        }
        
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(deathTimer);
        if (isPlayerDead) 
        {
            SceneManager.LoadScene("Demo_Level_0");
        }
    }
}
