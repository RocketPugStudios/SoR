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
    [SerializeField] public RaycastHit hit;
    [SerializeField] public GameObject weaponRaycast;


    private void Start()
    {
       // weapon = this.gameObject;
        weaponRaycast = FindObjectOfType<GameObject>(CompareTag("Raycasting Point"));
        if (weaponRaycast != null)
        {

        }
    }

    public void Update()
    {
      
        if (Physics.Raycast(weaponRaycast.transform.position, weaponRaycast.transform.forward, out hit, raycastDistance))
        {
            Debug.Log("hitting");
            Debug.DrawLine(weaponRaycast.transform.position, hit.point, Color.red);
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
