using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator MyAnimator;
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private string IdleState = "IsIdle";
    [SerializeField] private string WalkingState = "IsWalking";
    [SerializeField] private string DashingState = "IsDashing";

    int HashedIdleState = 0;
    int HashedWalkingState = 0;
    private int HashedDashingState = 0;
    void Start()
    {
        HashedIdleState = Animator.StringToHash(IdleState);
        HashedWalkingState = Animator.StringToHash(WalkingState);
        HashedDashingState = Animator.StringToHash(DashingState);
    }
    void Update()
    {
        UpdateAnimationState();
    }
    
    void UpdateAnimationState()
    {
        PlayerController PlayerController = GameManager.Instance.GetPlayerControllerRef;
        PackedMovementMode.EMovementModes CurrentMovementMode = PlayerController.GetCurrentMovementMode().MovementState;
        
        switch (CurrentMovementMode)
        {
            case PackedMovementMode.EMovementModes.Idle:
            {
                SetIdleState();
                break;
            }
            case PackedMovementMode.EMovementModes.Walking:
            {
                SetWalkingState(PlayerController.GetCurrentVelocity());
                break;
            }
            case PackedMovementMode.EMovementModes.Dashing:
            {
                SetDashingState();
                break;
            }
        }
    }

    void SetIdleState()
    {
        MyAnimator.SetBool(HashedIdleState, true);
        MyAnimator.SetBool(HashedWalkingState, false);
        MyAnimator.SetBool(HashedDashingState, false);
    }

    void SetWalkingState(Vector2 CurrentVelocity)
    {
        MyAnimator.SetBool(HashedIdleState, false);
        MyAnimator.SetBool(HashedWalkingState, false);
        MyAnimator.SetBool(HashedDashingState, false);

        SpriteRenderer.flipY = CurrentVelocity.x >= 0;
    }

    void SetDashingState()
    {
        MyAnimator.SetBool(HashedIdleState, false);
        MyAnimator.SetBool(HashedWalkingState,false);
        MyAnimator.SetBool(HashedDashingState, true);
    }
}
