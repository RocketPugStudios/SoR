using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class enemyWeapon : MonoBehaviour
{
    [Header("Relationships")]
    [SerializeField] public NPC_behavior_StateMachine NPC;

    [Header("Weapon Settings")]
    [SerializeField] public int round;
    [SerializeField] public int roundsinWeapon;
    [SerializeField] public int magazineSize;
    [SerializeField] public GameObject weapon;
    [SerializeField] public float weaponRecoil;
    [SerializeField] public float weaponDamage;
    //public RaycastHit hit;
   
    [SerializeField] public float raycastDistance;
    [SerializeField] public float rateOfFire;
    [SerializeField] public RaycastHit hit;


    private void Start()
    {
        weapon = GetComponentInChildren<GameObject>(CompareTag("enemyWeapon"));

    }

    public void Update()
    {
        
       //RaycastHit hit;

        if (Physics.Raycast(weapon.transform.position, transform.forward, out hit, raycastDistance))
        {
            //Debug.Log("hitting");
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        
    }

    public void shootPlayer()
    {
        if (hit.collider.CompareTag("Player"))
        {
            Debug.Log("hitplayer");
        }
    }
   


}
