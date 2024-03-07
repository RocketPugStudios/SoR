using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class enemyWeapon : MonoBehaviour
{
    [Header("Relationships")]
    public NPC_behavior_StateMachine NPC;

    [Header("Weapon Settings")]
    public int roundsInWeapon;
    public GameObject weapon;
    public float weaponRecoil;
    public float weaponDamage;
    //public RaycastHit hit;
    public List<string> firingMode;
    public List<Transform> firingPosition;
    public float raycastDistance;


    private void Start()
    {
        
    }

    public void Update()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            Debug.Log("hitting");
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        
    }

   


}
