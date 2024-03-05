using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator MyAnimator;
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [Header("Animator Parameters")]
    [SerializeField][ShowOnly] private string IdleState = "IsIdle";
    [SerializeField][ShowOnly] private string WalkingState = "IsWalking";
    [SerializeField][ShowOnly] private string IdlingXValue = "xIdle";
    [SerializeField][ShowOnly] private string IdlingYValue = "yIdle";
    [SerializeField][ShowOnly] private string WalkingXValue = "xWalk";
    [SerializeField][ShowOnly] private string WalkingYValue = "yWalk";

    [Header("AnimationEvents")]
    [SerializeField] AudioSource AudioSource;

    int HashedIdleState = 0;
    int HashedWalkingState = 0;

    [SerializeField][ShowOnly] private PlayerController CachedPlayerController = null;

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

        MyAnimator.SetFloat(IdlingXValue, CachedPlayerController.LastInput.x);
        MyAnimator.SetFloat(IdlingYValue, CachedPlayerController.LastInput.y);

        MyAnimator.SetFloat(WalkingXValue, CachedPlayerController.CurrentMove.x);
        MyAnimator.SetFloat(WalkingYValue, CachedPlayerController.CurrentMove.y);


        switch (CurrentMovementMode)
        {
            case PackedMovementMode.EMovementModes.Idle:
                {
                    SetIdleState(PlayerController.IsIdling);
                    break;
                }
            case PackedMovementMode.EMovementModes.Walking:
                {
                    SetWalkingState(PlayerController.IsWalking);
                    break;
                }

        }
    }

    void SetIdleState(bool idle)
    {
        MyAnimator.SetBool(HashedIdleState, idle);
        MyAnimator.SetBool(HashedWalkingState, !idle);
    }

    void SetWalkingState(bool walk)
    {
        MyAnimator.SetBool(HashedIdleState, !walk);
        MyAnimator.SetBool(HashedWalkingState, walk);

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
