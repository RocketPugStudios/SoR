using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animation_MovementController : MonoBehaviour
{



    Player_Xbox_Controls playerInput;
    CharacterController characterController;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new Player_Xbox_Controls();
        characterController = GetComponent<CharacterController>();

        playerInput.CharacterControls.Move.started += Context =>
        {

            currentMovementInput = Context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;
            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;




        };

        playerInput.CharacterControls.Move.canceled += Context =>
        {

            currentMovementInput = Context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;
            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;


        };

        playerInput.CharacterControls.Move.performed += Context =>
        {

            currentMovementInput = Context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;
            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;


        };
    }
    // Update is called once per frame
    void Update()
    {
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
