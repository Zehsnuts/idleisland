using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputs : MonoBehaviour
{
    private PlayerVision _playerVision;
    private PlayerControls _playerControls;
    private PlayerHands _playerHands;

    //Controls
    private InputAction _useAction;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerVision = FindObjectOfType<PlayerVision>();
        _playerHands = FindObjectOfType<PlayerHands>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Land.Use.performed += Use;

        _playerControls.Land.Use.started += ThrowObject;
        _playerControls.Land.Throw.performed += ThrowObject;
        _playerControls.Land.Throw.canceled += ThrowObject;

    }
    private void OnDisable()
    {
        _playerControls.Disable();

        _playerControls.Land.Use.performed -= Use;

        _playerControls.Land.Use.started -= ThrowObject;
        _playerControls.Land.Throw.performed -= ThrowObject;
        _playerControls.Land.Throw.canceled -= ThrowObject;
    }

    private void ThrowObject(InputAction.CallbackContext context)
    {
        //Debug.Log(context);

        if (context.interaction is HoldInteraction)
            if (!context.canceled)
                _playerHands.ThrowObject(true);
            else
            {
                _playerHands.ThrowObject();
                _playerVision.playerTarget = null;
            }

        if (context.interaction is PressInteraction)
            _playerHands.ThrowObject();
    }

    private void Use(InputAction.CallbackContext context)
    {
         if (_playerVision.playerTarget == null)
        {
            Debug.Log("Nothing to grab.");
            return;
        }

         
         if(context.performed)
            _playerVision.playerTarget.GetComponent<Interactables>().InteractMe();
    }
}
