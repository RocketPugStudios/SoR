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
        Debug.Log("Shot weapon");

        RaycastHit hit;
        Vector3 gunshotWorldPosition = gunshotPosition.transform.position;

        // Calculate direction from gunshot position to target in world space
        Vector3 directionToTarget = (target - gunshotWorldPosition).normalized;

        // Perform raycast in world space
        if (Physics.Raycast(gunshotWorldPosition, directionToTarget, out hit, raycastDistance))
        {
            Debug.Log("Hit: " + hit.transform.name);

            PlaySound();
            muzzleFlashPoint(gunshotWorldPosition);
            gunShotParticle(hit.point, hit.normal, hit.collider.gameObject);
            Debug.DrawLine(gunshotWorldPosition, hit.point, Color.red);

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<enemyHealth>().OnHitByPlayer();
            }
        }
    }

    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(gunshot, transform.position);
    }

    private void gunShotParticle(Vector3 hitPoint, Vector3 normal, GameObject hitObject)
    {
        if (hitObject.CompareTag("Enemy"))
        {
            Instantiate(enemygunshot, hitPoint, Quaternion.LookRotation(normal));
        }
        else
        {
            Instantiate(wallgunshot, hitPoint, Quaternion.LookRotation(normal));
        }
    }

    private void muzzleFlashPoint(Vector3 weaponPoint)
    {
        ParticleSystem _muzzleflash = Instantiate(muzzleFlash, weaponPoint, Quaternion.LookRotation(Vector3.back));
        Destroy(_muzzleflash.gameObject, _muzzleflash.main.duration + _muzzleflash.main.startLifetime.constantMax);
    }
}
