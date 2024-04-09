using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animation_MovementController : MonoBehaviour
{
    [Header("Script Relationships")]
    [SerializeField] Player_Xbox_Controls playerInput;
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animations;
    
    [Header("Variables")]
    [SerializeField] private Vector2 currentMovementInput;
    [SerializeField] private Vector3 currentMovement;
    [SerializeField] private bool isMovementPressed;
    [SerializeField] private bool isAimPressed;
    [SerializeField] private float roatationFactorPerFrame = 15f;
    void Awake()
    {
        playerInput = new Player_Xbox_Controls();
        characterController = GetComponent<CharacterController>();
        animations = GetComponent<Animator>();

        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;

        playerInput.CharacterControls.Aim.started += OnAimInput;
        playerInput.CharacterControls.Aim.canceled += OnAimInput;
    }
    void OnAimInput(InputAction.CallbackContext context)
    {
        isAimPressed = context.ReadValueAsButton();
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    void handleAnimation()
    {
        bool isWalking = animations.GetBool("isWalking");
        //bool isRunning = animations.GetBool("isRunning");
        bool isAiming = animations.GetBool("isAiming");
        //movement handler
        if (isMovementPressed && !isWalking)
        {
            animations.SetBool("isWalking", true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animations.SetBool("isWalking", false);
        }
        //Aiming Handler
        if (isAimPressed)
        {
            animations.SetBool("isAiming", true);
        }
        else if (!isAimPressed) { animations.SetBool("isAiming", false);}
        // Aim and walk handler
        if (isMovementPressed && isAimPressed) {
            animations.SetBool("isWalking", true);
            animations.SetBool("isAiming", true);
        }
        else if (!isMovementPressed && !isAimPressed) 
        {
            animations.SetBool("isWalking", false);
            animations.SetBool("isAiming", false);
        }
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;
      
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, roatationFactorPerFrame * Time.deltaTime);
        }
    }
    void Update()
    {
        handleRotation();
        handleAnimation();
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
