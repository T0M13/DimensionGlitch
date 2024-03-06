using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingleton<InputManager>
{
    [Header("References")]
    [SerializeField] IM_Player inputActions;
    [SerializeField] InputActionReference[] SlotbarHotkeys;
    [SerializeField] private Camera mainCamera;
    [Header("Debug")]
    [SerializeField][Range(0, 1)] private float gizmosRadius = 0.5f;
    [SerializeField] private LayerMask buildingLayermask;
    [SerializeField][ShowOnly] private Vector3 lastPosition;
    [SerializeField][ShowOnly] private Vector3 mousePos;
    [SerializeField][ShowOnly] private Vector3 mousePosInWorld;
    public LayerMask BuildingLayermask { get => buildingLayermask; set => buildingLayermask = value; }

    public IM_Player InputActions { get => inputActions; set => inputActions = value; }
    public Vector3 GetMousePositionInWorld() => mousePosInWorld;
    public Vector3 GetMousePositionScreen() => mousePos;


    private void Start()
    {
        if(!Application.isPlaying) return;
        
        InputActions.Enable();
        EnableSlotbarHotkeys();
        InputActions.MouseControls.MousePosition.performed += MousePositionPerformed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(!Application.isPlaying) return;
        
        InputActions.Disable();
        DisableSlotbarHotkeys();
        InputActions.MouseControls.MousePosition.performed -= MousePositionPerformed;

    }

    //Enzo M
    void EnableSlotbarHotkeys()
    {
        foreach (var SlotbarHotkey in SlotbarHotkeys)
        {
            SlotbarHotkey.action.Enable();
        }
    }

    //Enzo M
    void DisableSlotbarHotkeys()
    {
        foreach (var SlotbarHotkey in SlotbarHotkeys)
        {
            SlotbarHotkey.action.Disable();
        }
    }

    //Enzo M
    public bool IsShiftPressed()
    {
        return inputActions.ComboKeys.ShiftComboKey.IsPressed();
    }
    
    //Enzo M
    public bool IsAltPressed()
    {
        return InputActions.ComboKeys.AltComboKey.IsPressed();
    }
    
    private void OnValidate()
    {
        mainCamera = Camera.main;
    }

    protected override void Awake()
    {
        base.Awake();
        inputActions = new IM_Player();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        mousePosInWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));
        mousePosInWorld.z = 0;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mousePosInWorld, gizmosRadius);
    }

    private void MousePositionPerformed(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }
}
