using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingleton<InputManager>
{
    [Header("References")]
    [SerializeField] IM_Player inputActions;
    [SerializeField] private Camera mainCamera;
    [Header("Settings")]
    [SerializeField] private LayerMask placementLayermask;
    [Header("Debug")]
    [SerializeField][Range(0, 1)] private float gizmosRadius = 0.5f;
    [SerializeField][ShowOnly] private Vector3 lastPosition;
    [SerializeField][ShowOnly] private Vector3 mousePos;
    [SerializeField][ShowOnly] private Vector3 mousePosInWorld;


    public IM_Player InputActions { get => inputActions; set => inputActions = value; }
    public Vector3 GetMousePositionInWorld() => mousePosInWorld;

    private void OnEnable()
    {
        InputActions.Enable();
        InputActions.MouseControls.MousePosition.performed += MousePositionPerformed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputActions.Disable();
        InputActions.MouseControls.MousePosition.performed -= MousePositionPerformed;

    }

    private void OnValidate()
    {
        //inputActions = new IM_Player();
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

    //public Vector3 GetSelectedMapPosition()
    //{
    //    mousePos.z = mainCamera.nearClipPlane;
    //    Ray ray = mainCamera.ScreenPointToRay(mousePos);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, 100, placementLayermask))
    //    {
    //        lastPosition = hit.point;
    //    }
    //    return lastPosition;
    //}

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
