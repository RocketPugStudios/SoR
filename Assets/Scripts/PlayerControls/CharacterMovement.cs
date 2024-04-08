using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
   [SerializeField] public CharacterController player;
    Rigidbody rigidbody;
    public float speed = 4;
    Vector3 lookPos;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0; // Keep the direction in the horizontal plane

        transform.LookAt(transform.position + lookDir, Vector3.up);
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Convert input into a movement vector based on the camera's orientation
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * movement; // Adjust movement direction based on camera rotation

        rigidbody.AddForce(movement * speed / Time.deltaTime);
    }
}
