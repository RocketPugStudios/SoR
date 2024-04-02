using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder;

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
            shootWeapon();
        } 
    }

    public void shootWeapon()
    {
        RaycastHit hit;

        if (Physics.Raycast(weaponRaycast.transform.position, weaponRaycast.transform.forward, out hit, raycastDistance))
        {
            Debug.Log("hitting");
            PlaySound();
            muzzleFlashPoint(weaponRaycast.position);
            gunShotParticle(hit.point, hit.normal);
            Debug.DrawLine(weaponRaycast.transform.position, hit.point, Color.red);
        }
    }

    void PlaySound()
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

    void gunShotParticle(Vector3 hitObject, Vector3 normal)
    {
        Instantiate(wallgunshot,hitObject,Quaternion.LookRotation(normal));
    }

    void muzzleFlashPoint(Vector3 weaponPoint)
    {
        ParticleSystem _muzzleflash = Instantiate(muzzleFlash, weaponPoint, Quaternion.LookRotation(Vector3.back));
        
        Destroy (_muzzleflash.gameObject, _muzzleflash.main.duration + _muzzleflash.main.startLifetime.constantMax);
    }

}
