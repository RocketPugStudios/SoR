using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public Transform shootingPoint; // Assign the cube's transform here.
    public float shootingRange = 100f;
    public int magazineCapacity = 30;
    private int currentAmmo;
    private List<int> magazines = new List<int>();
    private int currentMagazineIndex = 0;
    public TMPro.TextMeshProUGUI ammoDisplay; 
    public TMPro.TextMeshProUGUI magazineDisplay;




    private void Start()
    {
        // Initialize with 3 magazines.
        for (int i = 0; i < 3; i++)
        {
            magazines.Add(magazineCapacity);
        }
        // Set currentAmmo to the first magazine's capacity at the start.
        currentAmmo = magazines[currentMagazineIndex];
        UpdateAmmoDisplay();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentAmmo > 0)
        {
            Shoot();
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

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            RaycastHit hit;
            Debug.DrawRay(shootingPoint.position, shootingPoint.forward * shootingRange, Color.red, 2.0f);

            if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, shootingRange))
            {
                Debug.Log($"Hit: {hit.transform.name}");
            }
            else
            {
                Debug.Log("Missed");
            }

            UpdateAmmoDisplay();
        }

        // Automatically reload when out of ammo.
        if (currentAmmo <= 0 && magazines.Count > 1)
        {
            Reload();
        }
    }


    void Reload()
    {
        // Check if there are magazines left to switch to.
        if (magazines.Count > 1)
        {
            // Save the current ammo count back to the current magazine.
            magazines[currentMagazineIndex] = currentAmmo;

            // Move to the next magazine.
            currentMagazineIndex = (currentMagazineIndex + 1) % magazines.Count;

            // Load the ammo count from the new magazine.
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
