using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Stats))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Stats PlayerStats;
    
    [Header("Walk")]
    [SerializeField, Min(0)] float Speed = 10;//meters
    [SerializeField, Min(0)] float TimeToReachMaxSpeed = 1;
    [SerializeField] Rigidbody2D PlayerRb = null;

    [Header("Dash")] 
    [SerializeField, Min(0)] float DashDistance = 50;
    [SerializeField, Min(0)] float DashTime = 10;//meters
    [SerializeField, Min(0)] float DashCd = 10;//meters
    
    [Header("InputActions")] 
    [SerializeField] InputActionReference Walk;
    [SerializeField] InputActionReference Dash;

    [Header("Debug")] 
    [SerializeField] float MovementTime = 0.0f;
    [SerializeField] float LastDashTime = 0.0f;
    [SerializeField] Vector2 CurrentVelocity = Vector2.zero;

    private float TravelledDashDistance = 0.0f;
    PackedMovementMode CurrentMovementMode = MovementModeFactory.GetDefaultMovementMode();

    public float GetDashProgress() => CurrentMovementMode.MovementState == PackedMovementMode.EMovementModes.Dashing ? TravelledDashDistance / DashDistance : 0;
    public float GetDashCD() => DashCd;
    public PackedMovementMode GetCurrentMovementMode() => CurrentMovementMode;
    public Vector2 GetCurrentVelocity() => CurrentVelocity;
    public Stats GetPlayerStats() => PlayerStats;
    public event Action OnDash;
    
    void Start()
    {
        Dash.action.performed += DoDash;
        Walk.action.canceled += ResetMovementState;
    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnDisable()
    {
        Dash.action.performed -= DoDash;
        Walk.action.canceled -= ResetMovementState;
    }
    void ResetMovementState(InputAction.CallbackContext _)
    {
        CurrentMovementMode = MovementModeFactory.GetDefaultMovementMode();
        CurrentVelocity = Vector2.zero;
        MovementTime = 0.0f;
    }
    
#region Walking

    void MovePlayer()
    {
        //Get current player pos
        Vector2 CurrentPlayerPosition = PlayerRb.position;
        Vector2 CurrentPlayerInput = Walk.action.ReadValue<Vector2>();

        //if the player is moving move him if the player cant walk right now dont do anything
        if(!CurrentMovementMode.bCanWalk) return;
        if (!CurrentPlayerInput.Equals(Vector2.zero))
        {
            MovementTime += Time.deltaTime;
            //add the movement vector multiplied by the progress of acceleration time
            float SpeedMultiplier = (Mathf.Clamp01(MovementTime / TimeToReachMaxSpeed));
            CurrentVelocity = CurrentPlayerInput * (Speed * Time.deltaTime * SpeedMultiplier);
           
            PlayerRb.MovePosition(CurrentPlayerPosition + CurrentVelocity);
            CurrentMovementMode = MovementModeFactory.GetWalkingMovementMode();
        }
    }

#endregion

#region Dash

    void DoDash(InputAction.CallbackContext CallbackContext)
    {
        if(!IsAllowedToDash()) return;
        
        StartCoroutine(DashRoutine());
        
        LastDashTime = Time.time;
        CurrentMovementMode = MovementModeFactory.GetDashMovementMode();
        
        OnDash?.Invoke();
    }

    IEnumerator DashRoutine()
    {
        //float Travelled Distance maybe we can implement this for a blend tree to set animation frame
        float DashSpeed = DashDistance / DashTime;
        float PassedTime = 0.0f;
        Vector2 DashDirection = CurrentVelocity.normalized;
        
        while (PassedTime < DashTime)
        {
            PlayerStats.SetInvincibility(true);
            
            //Get the dash movement
            Vector2 DashVector = DashDirection * (DashSpeed * Time.deltaTime);
            //Get the current player pos
            Vector2 PreviousPlayerPos = PlayerRb.position;
            //Get the new pos for the player dash vector added
            Vector2 NewPlayerPos = PreviousPlayerPos + DashVector;

            TravelledDashDistance += Vector2.Distance(PreviousPlayerPos, NewPlayerPos);
            //Move the player rb to the new position
            PlayerRb.MovePosition(NewPlayerPos);
            PassedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        CurrentMovementMode = MovementModeFactory.GetDefaultMovementMode();
        PlayerStats.SetInvincibility(false);
        TravelledDashDistance = 0;
    }
    bool IsAllowedToDash()
    {
        return CurrentMovementMode.bCanDash && Time.time - LastDashTime > DashCd;
    }

#endregion
}
