using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // The player's transform
    public Vector3 offset; // Offset distance between the player and camera
    public float rotationSpeed = 5.0f; // Speed at which the camera rotates

    void LateUpdate()
    {
        // Check if the player exists to avoid errors
        if (playerTransform != null)
        {
            // Camera follows the player with specified offset position
            Vector3 desiredPosition = playerTransform.position + offset;
            transform.position = desiredPosition;

            // Rotate the camera around the player based on mouse input
            if (Input.GetMouseButton(1)) // Right mouse button by default, change if needed
            {
                Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
                offset = camTurnAngle * offset;
            }

            // Always look at the player
            transform.LookAt(playerTransform);
        }
    }
}
