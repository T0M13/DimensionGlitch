using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingleton<InputManager>
{
    [Header("InputActions")] 
    [SerializeField] InputActionReference PlayerWalk;
    [SerializeField] InputActionReference PlayerDash;
    
    private void Start()
    {
        EnableDefaultActions();
    }

    private void OnDisable()
    {
        DisableAllActions();
    }

    void EnableDefaultActions()
    {
        PlayerWalk.action.Enable();
        PlayerDash.action.Enable();
    }

    public void DisableAllActions()
    {
        PlayerWalk.action.Disable();
        PlayerDash.action.Disable();
    }
    
    public void SeMovementEnabled()
    {
        PlayerWalk.action.Enable();
    }

    public void SetMovementDisabled()
    {
        PlayerWalk.action.Disable();
    }
}
