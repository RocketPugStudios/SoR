using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animation_MovementController : MonoBehaviour
{



    Player_Xbox_Controls playerInput;


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new Player_Xbox_Controls();
        playerInput.CharacterControls.Move.started += Context => { };

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        
    }
}
