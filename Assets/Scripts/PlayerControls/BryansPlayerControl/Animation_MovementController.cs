using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Animation_MovementController : MonoBehaviour
{
    [Header("Script Relationships")]
    [SerializeField] Player_Xbox_Controls playerInput;
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animations;
    [SerializeField] Camera playerCamera;  // Reference to the Camera
     playerWeaponPosition primaryWeapon;
    [SerializeField] MMF_Player MMFfeedback; // Reference to the Camera

    [Header("Variables")]
    [SerializeField] private Vector2 currentMovementInput;
    //Movement
    [SerializeField] private Vector3 currentMovement;
    [SerializeField] private Vector3 currentRunMovement;
    //button Presses
    [SerializeField] private bool isMovementPressed;
    [SerializeField] private bool isAimPressed;
    [SerializeField] private bool isShootPressed;
    [SerializeField] private bool isAimingWhileWalkingPressed;
    //quaternion
    [SerializeField] private float rotationFactorPerFrame = 15f;
    [SerializeField] private float rotationSpeed = 5f;
    //Misc
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector3 target;
   



    void Awake()
    {
        playerInput = new Player_Xbox_Controls();
        characterController = GetComponent<CharacterController>();
        primaryWeapon = GetComponentInChildren<playerWeaponPosition>();

        searchForCamera();
        animations = GetComponent<Animator>();

        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;

        playerInput.CharacterControls.Aim.started += OnAimInput;
        playerInput.CharacterControls.Aim.canceled += OnAimInput;

        playerInput.CharacterControls.Shoot.started += OnShootInput;
        playerInput.CharacterControls.Shoot.performed += OnShootInput;
        playerInput.CharacterControls.Shoot.canceled += OnShootInput;

    }

    void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;


        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;

        }
    }
    void HandleFireWeapon()
    {
        
        // Check if the shoot button was pressed and the aim button is also pressed
        if (isShootPressed && isAimPressed)
        {
            MMFfeedback.PlayFeedbacks();
            Debug.Log("calling gunshot");
            primaryWeapon.GunShot(target);
            // Reset isShootPressed to false to ensure it only fires once per button press
            isShootPressed = false;
        }
    }

    void OnShootInput(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            isShootPressed = true;
        }
        // Optionally handle the button release if needed for other logic
        if (context.canceled)
        {
            isShootPressed = false;
        }
    }
    void searchForCamera()
    {
        if (playerCamera == null)
        {
            Debug.Log("finding camera");
            foreach (GameObject cam in FindObjectsOfType<GameObject>())
            {
                Debug.Log(cam);
                if (cam.CompareTag("Camera"))
                {
                    Debug.Log("Camera found");
                    playerCamera = cam.GetComponent<Camera>();
                    break;
                }
            }
        }

    }

    void OnAimInput(InputAction.CallbackContext context)
    {
        isAimPressed = context.ReadValueAsButton();
       
    }

    void Aim()
    {
        if (isAimPressed)
        {

            var (success, position) = GetMousePosition();

            if (success)
            {
                var direction = position - transform.position;
                direction.y = 0;  // Ensure there's no vertical rotation.
                transform.forward = direction.normalized;
            }


            (bool success, Vector3 position) GetMousePosition()
            {
                var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
               

                if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
                {
                    target = hitInfo.point;
                    
                    // The Raycast hit something, return with the position.
                    return (success: true, position: hitInfo.point);
                    
                }
                else
                {
                    // The Raycast did not hit anything.
                    return (success: false, position: Vector3.zero);
                }
            }
        }
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        UpdateMovement();
        animations.SetFloat("x",currentMovementInput.x == 0 ? 0 : Mathf.Sign( currentMovement.x));
    }

    void UpdateMovement()
    {
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        if (isAimPressed)
        {
            // Combine forward and lateral movement
            currentMovement = (forward * currentMovementInput.y + right * currentMovementInput.x).normalized;
        }
        else
        {
            // Regular movement in all directions
            currentMovement = (forward * currentMovementInput.y + right * currentMovementInput.x).normalized;
        }
    }



    void HandleAnimation()
    {
        bool isWalking = isMovementPressed && !isAimPressed;
        bool isAiming = isAimPressed;
        bool isAimingWhileWalking = isMovementPressed && isAimPressed;


        animations.SetBool("isWalking", isWalking);
        animations.SetBool("isAiming", isAiming);
        animations.SetBool("isAimingWhileWalking", isAimingWhileWalking);
       
    }

    void HandleRotation()
    {
        if (isMovementPressed && !isAimPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void Update()
    {
        HandleRotation();
        HandleAnimation();
       // HandleGravity();
        Aim();
        HandleFireWeapon();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
