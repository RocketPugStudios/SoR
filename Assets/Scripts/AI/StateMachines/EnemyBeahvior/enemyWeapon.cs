using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngineInternal;

public class enemyWeapon:MonoBehaviour
{
    [Header("Relationships")]
    [SerializeField] public NPC_behavior_StateMachine NPC;
    [Header("Weapon Settings")]
    [SerializeField] public GameObject weapon;
    [SerializeField] public float raycastDistance;
    
    
    [SerializeField] public Transform weaponRaycast;
    [Header("Sound")]
    [SerializeField] public AudioClip gunshot;

    [Header("Particle Effects")]
    [SerializeField] public ParticleSystem wallgunshot;
    [SerializeField] public ParticleSystem muzzleFlash;
    [SerializeField] public ParticleSystem playergunshot;



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
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootWeapon();
        }
        */
    }

    public void shootWeapon()
    {
        RaycastHit hit;

        if (Physics.Raycast(weaponRaycast.transform.position, weaponRaycast.transform.forward, out hit, raycastDistance))
        {
            Debug.Log("hitting");
            PlaySound();
            muzzleFlashPoint(weaponRaycast.position);
            gunShotParticle(hit.point, hit.normal, hit.collider.gameObject);
            Debug.DrawLine(weaponRaycast.transform.position, hit.point, Color.red);

            if (hit.collider.CompareTag("Player"))
            {
                GameObject player = hit.collider.gameObject;

                player.GetComponent<playerHealthSystem>().playerConstitution();

            }
            
        }
    }

   

   private void PlaySound()
    {
        // Create a new GameObject
        GameObject soundObject = new GameObject("Sound");
        // Add an AudioSource component dynamically and configure it
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = gunshot;

        audioSource.Play();

        // Destroy the GameObject after the clip has finished playing
        Destroy(soundObject, gunshot.length);
    }

   private void gunShotParticle(Vector3 hitPoint, Vector3 normal, GameObject hitObject)
    {
        //Instantiate(wallgunshot,hitPoint,Quaternion.LookRotation(normal));
        //void pickParticleEffect()
        {
            if (hitObject.CompareTag("Player")){Instantiate(playergunshot,hitPoint,Quaternion.LookRotation(normal));}
           else Instantiate(wallgunshot, hitPoint, Quaternion.LookRotation(normal));
        }     
    }

    private void muzzleFlashPoint(Vector3 weaponPoint)
    {
        ParticleSystem _muzzleflash = Instantiate(muzzleFlash, weaponPoint, Quaternion.LookRotation(Vector3.back));
        
        Destroy (_muzzleflash.gameObject, _muzzleflash.main.duration + _muzzleflash.main.startLifetime.constantMax);
    }

  
}
