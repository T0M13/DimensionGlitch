using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Stats))]
public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Stats PlayerStats;
    [SerializeField] IM_Player inputActions;
    [SerializeField] Rigidbody2D playerRB = null;

    [Header("Walk")]
    [SerializeField, Min(0)] float moveSpeed = 5;

    [Header("Debug")]
    [SerializeField][ShowOnly] Vector2 currentInput = Vector2.zero;
    [SerializeField][ShowOnly] Vector2 currentMove = Vector2.zero;

    PackedMovementMode CurrentMovementMode = MovementModeFactory.GetDefaultMovementMode();

    public Vector2 CurrentInput() => currentInput;
    public PackedMovementMode GetCurrentMovementMode() => CurrentMovementMode;
    public Stats GetPlayerStats() => PlayerStats;


    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.PlayerControlls.Walk.performed += WalkPerformed;
        inputActions.PlayerControlls.Walk.canceled += WalkCancelled;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.PlayerControlls.Walk.performed -= WalkPerformed;
        inputActions.PlayerControlls.Walk.canceled -= WalkCancelled;
    }


    private void OnValidate()
    {
        playerRB = GetComponent<Rigidbody2D>();
        inputActions = new IM_Player();
    }

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        inputActions = new IM_Player();
    }

    private void Update()
    {
        currentMove = new Vector2(currentInput.x, currentInput.y) * moveSpeed;
    }

    private void FixedUpdate()
    {
        playerRB.velocity = currentMove;
    }

    public void WalkPerformed(InputAction.CallbackContext context)
    {
        currentInput = context.ReadValue<Vector2>();
    }

    public void WalkCancelled(InputAction.CallbackContext context)
    {
        currentInput = Vector2.zero;
    }
}
