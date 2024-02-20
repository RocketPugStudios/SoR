using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // The player's transform
    public Vector3 offset; // Offset distance between the player and camera

    void LateUpdate()
    {
        // Check if the player exists to avoid errors
        if (playerTransform != null)
        {
            // Set the camera's position to the player's position plus the offset
            transform.position = playerTransform.position + offset;
        }
    }
}
