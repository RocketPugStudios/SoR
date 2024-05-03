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

    // Ammo and reload variables
    private int currentAmmo;
    private List<int> magazines = new List<int>();
    private int currentMagazineIndex = 0;
    public TMPro.TextMeshProUGUI ammoDisplay;
    public TMPro.TextMeshProUGUI magazineDisplay;
    public int magazineCapacity = 30; // Set this to your magazine capacity

    void Awake()
    {
        movementController = GetComponentInParent<Animation_MovementController>();

        // Initialize with 3 magazines
        for (int i = 0; i < 3; i++)
        {
            magazines.Add(magazineCapacity);
        }
        // Set currentAmmo to the first magazine's capacity at the start
        currentAmmo = magazines[currentMagazineIndex];
        UpdateAmmoDisplay();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentAmmo > 0)
        {
           //GunShot(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane)));
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // Example key for changing magazines, improve as needed.
        if (Input.GetKeyDown(KeyCode.T))
        {
            CycleMagazines();
        }
    }

    public void GunShot(Vector3 target)
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of Ammo!");
            return; // Block shooting when out of ammo
        }

        currentAmmo--;
        UpdateAmmoDisplay();

        Debug.Log("Shot weapon");
        RaycastHit hit;
        Vector3 gunshotWorldPosition = gunshotPosition.transform.position;
        Vector3 directionToTarget = (target - gunshotWorldPosition).normalized;

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

    void Reload()
    {
        if (magazines.Count > 1)
        {
            magazines[currentMagazineIndex] = currentAmmo;
            currentMagazineIndex = (currentMagazineIndex + 1) % magazines.Count;
            currentAmmo = magazines[currentMagazineIndex];
            UpdateAmmoDisplay();
        }
    }

    void CycleMagazines()
    {
        if (magazines.Count > 1)
        {
            magazines[currentMagazineIndex] = currentAmmo;
            currentMagazineIndex = (currentMagazineIndex + 1) % magazines.Count;
            currentAmmo = magazines[currentMagazineIndex];
            UpdateAmmoDisplay();
        }
    }

    void UpdateAmmoDisplay()
    {
        ammoDisplay.SetText($"Ammo: {currentAmmo}/{magazineCapacity}");
        magazineDisplay.SetText($"Magazine: {currentMagazineIndex + 1}/{magazines.Count}");
    }
}
