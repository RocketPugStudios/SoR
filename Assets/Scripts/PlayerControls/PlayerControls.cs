using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject grenade;
    public float throwForce = 80f;
    public float throwOffset = 1f;

    void Update()
    {
        // Weapon Swap with Mouse Scroll Wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Debug.Log("Switched to Next Weapon");
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Debug.Log("Switched to Previous Weapon");
        }

        // Weapon Swap with Number Keys 1, 2, 3, 4, 5
        if (Input.GetKeyDown(KeyCode.Alpha1)) Debug.Log("Switched to Weapon 1");
        if (Input.GetKeyDown(KeyCode.Alpha2)) Debug.Log("Switched to Weapon 2");
        if (Input.GetKeyDown(KeyCode.Alpha3)) Debug.Log("Switched to Weapon 3");
        if (Input.GetKeyDown(KeyCode.Alpha4)) Debug.Log("Switched to Weapon 4");
        if (Input.GetKeyDown(KeyCode.Alpha5)) Debug.Log("Switched to Weapon 5");

        // Fire Weapon
        if (Input.GetButtonDown("Fire1")) // Default left mouse button
        {
            Debug.Log("Weapon Fired");
        }

        // Aim Button - Toggle with Right Mouse Button
        if (Input.GetButtonDown("Fire2")) // On press
        {
            Debug.Log("Started Aiming");
        }
        else if (Input.GetButtonUp("Fire2")) // On release
        {
            Debug.Log("Stopped Aiming");
        }

        // Crouch Button
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Crouched");
        }

        // Lean Left
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Leaning Left");
        }

        // Lean Right
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Leaning Right");
        }

        // Throw Grenade
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }

        // Activate Ability
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Ability Activated");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Stealth Walk");
        }

        // Sprint with Shift Key
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Debug.Log("Sprinting");
        }
        
    }
    void ThrowGrenade()
    {
        Vector3 spawnPosition = transform.position + transform.forward * throwOffset;

        // Instantiate the grenade at the calculated spawn position
        Instantiate(grenade, spawnPosition, transform.rotation);

        // Get the Rigidbody component of the grenade
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        // Apply force to the grenade in the forward direction of the player
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        Debug.Log("Grenade Thrown");
    }
}
