using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class enemyWeapon:MonoBehaviour
{
    [Header("Relationships")]
    [SerializeField] public NPC_behavior_StateMachine NPC;
    [Header("Weapon Settings")]
    [SerializeField] public GameObject weapon;
    [SerializeField] public float raycastDistance;
    
    [SerializeField] public Transform weaponRaycast;
    [Header("Sound")]
    [SerializeField] public AudioSource singleFireSound;
    [SerializeField] public AudioSource BurstFireSound;

    private void Start()
    {
      
        foreach (Transform child in transform)
        {
            Debug.Log(child);
            if (child.CompareTag("Raycasting Point"))
            {
                weaponRaycast = child;
            }
        }   
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootPlayer();
        }
       
    }

    public void shootPlayer()
    {
        RaycastHit hit;

        if (Physics.Raycast(weaponRaycast.transform.position, weaponRaycast.transform.forward, out hit, raycastDistance))
        {
            Debug.Log("hitting");
            singleFireSound.Play();
            Debug.DrawLine(weaponRaycast.transform.position, hit.point, Color.red);
        }
    }
   


}
