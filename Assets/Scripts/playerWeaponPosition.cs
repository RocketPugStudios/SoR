using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponPosition : MonoBehaviour
{
    [SerializeField] GameObject gunshotPosition;
    [SerializeField] float raycastDistance;
   
     Animation_MovementController movementController;


    [Header("Relationships")]
    [SerializeField] public GameObject player;

    [Header("Weapon Settings")]
    

    [Header("Sound")]
    [SerializeField] AudioClip gunshot;

    [Header("Particle Effects")]
    [SerializeField] public ParticleSystem wallgunshot;
    [SerializeField] public ParticleSystem muzzleFlash;
    [SerializeField] public ParticleSystem enemygunshot;

    void Awake()
    {
        movementController = GetComponentInParent<Animation_MovementController>();
    }

    public void GunShot(Vector3 target)
    {
        Debug.Log("SHot weapon");
        RaycastHit hit;


        Vector3 directionToTarget = (target - transform.position).normalized;

        if (Physics.Raycast(gunshotPosition.transform.position, directionToTarget,out hit, raycastDistance)) 
        {
            Debug.Log("hit" + hit.transform);
            PlaySound();
            muzzleFlashPoint(gunshotPosition.transform.position);
            gunShotParticle(hit.point, hit.normal, hit.collider.gameObject);
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
            if (hitObject.CompareTag("Enemy")) { Instantiate(enemygunshot, hitPoint, Quaternion.LookRotation(normal)); }
            else Instantiate(wallgunshot, hitPoint, Quaternion.LookRotation(normal));
        }
    }

    private void muzzleFlashPoint(Vector3 weaponPoint)
    {
        ParticleSystem _muzzleflash = Instantiate(muzzleFlash, weaponPoint, Quaternion.LookRotation(Vector3.back));

        Destroy(_muzzleflash.gameObject, _muzzleflash.main.duration + _muzzleflash.main.startLifetime.constantMax);
    }
}
