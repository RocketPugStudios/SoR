using System.Collections;
using System.Collections.Generic;
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
    public RaycastHit hit;
    public List<string> firingMode;
    public List<Transform> firingPosition;
     
}
