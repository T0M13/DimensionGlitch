using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator MyAnimator;
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private string IdleState = "IsIdle";
    [SerializeField] private string WalkingState = "IsWalking";

    [Header("AnimationEvents")]
    [SerializeField] AudioSource AudioSource;
    
    int HashedIdleState = 0;
    int HashedWalkingState = 0;

    private PlayerController CachedPlayerController = null;
    
    void Start()
    {
        HashedIdleState = Animator.StringToHash(IdleState);
        HashedWalkingState = Animator.StringToHash(WalkingState);

        CachedPlayerController = GameManager.Instance.GetPlayerControllerRef;
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
                //SetWalkingState(PlayerController.GetCurrentVelocity());
                break;
            }

        }
    }

    void SetIdleState()
    {
        MyAnimator.SetBool(HashedIdleState, true);
        MyAnimator.SetBool(HashedWalkingState, false);
        // MyAnimator.SetBool(HashedDashingState, false);
    }

    void SetWalkingState(Vector2 CurrentVelocity)
    {
        MyAnimator.SetBool(HashedIdleState, false);
        MyAnimator.SetBool(HashedWalkingState, true);
        // MyAnimator.SetBool(HashedDashingState, false);
        
        if (CurrentVelocity.x != 0)
        {
            SpriteRenderer.flipX = CurrentVelocity.x <= 0;
        }
    }


#region AnimationEventFunctions

public void PlaySound(AudioClip ClipToPlay)
{
    if (AudioSource)
    {
        AudioSource.PlayOneShot(ClipToPlay, AudioSource.volume);
    }
}    

#endregion

}
