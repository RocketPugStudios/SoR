using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutout : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // Reference to the player object that the cutout effect will center on

    [SerializeField] private LayerMask wallMask; // Layer mask to filter which objects to apply the cutout effect

    public Camera mainCamera; // Reference to the main camera

    private void Awake()
    {
        // Ensure the main camera is correctly assigned, if not, attempt to find the main camera in the scene
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Convert the player's world position to a viewport point
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        // Adjust the Y coordinate of cutoutPos to account for different aspect ratios
        cutoutPos.y /= (Screen.width / Screen.height);

        // Use Physics.RaycastAll to detect all objects between the camera and the player, based on the defined wallMask
        Vector3 directionToTarget = targetObject.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToTarget, directionToTarget.magnitude, wallMask);

        // Process each hit object
        foreach (RaycastHit hit in hits)
        {
            // Check if the object has a Renderer component
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Loop through each material of the renderer
                foreach (Material material in renderer.materials)
                {
                    // Update the cutout position, size, and falloff parameters for the shader
                    material.SetVector("_CutoutPos", cutoutPos);
                    material.SetFloat("_CutoutSize", 0.1f);
                    material.SetFloat("_FalloffSize", 0.05f);
                }
            }
        }
    }
}
