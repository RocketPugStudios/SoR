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

            // Rotate the camera around the player based on Q and E input
            if (Input.GetKey(KeyCode.Q)) // Rotate left with Q key
            {
                RotateCamera(-rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E)) // Rotate right with E key
            {
                RotateCamera(rotationSpeed * Time.deltaTime);
            }

            // Always look at the player
            transform.LookAt(playerTransform);
        }
    }

    void RotateCamera(float angle)
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(angle, Vector3.up);
        offset = camTurnAngle * offset;
    }
}
