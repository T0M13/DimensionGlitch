using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Stats))]
public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] PlayerStats PlayerStats;
    [SerializeField] Rigidbody2D playerRB = null;

    [Header("Movement")]
    [SerializeField, Min(0)] float moveSpeed = 5;

    [Header("Debug")]
    [SerializeField][ShowOnly] bool isIdling;
    [SerializeField][ShowOnly] bool isWalking;
    [SerializeField][ShowOnly] Vector2 lastInput = Vector2.down;
    [SerializeField][ShowOnly] Vector2 currentInput = Vector2.zero;
    [SerializeField][ShowOnly] Vector2 currentMove = Vector2.zero;


    [SerializeField][ShowOnly] private InputManager CachedInputManager = null;

    PackedMovementMode CurrentMovementMode = MovementModeFactory.GetDefaultMovementMode();

    public bool IsIdling { get => isIdling; set => isIdling = value; }
    public bool IsWalking { get => isWalking; set => isWalking = value; }
    public Vector2 LastInput => lastInput;
    public Vector2 CurrentInput() => currentInput;
    public Vector2 CurrentMove { get => currentMove; set => currentMove = value; }

    public PackedMovementMode GetCurrentMovementMode() => CurrentMovementMode;
    public PlayerStats GetPlayerStats() => PlayerStats;


    private void OnEnable()
    {
        InputManager.Instance.InputActions.PlayerControls.Walk.performed += WalkPerformed;
        InputManager.Instance.InputActions.PlayerControls.Walk.canceled += WalkCancelled;
    }

    private void OnDisable()
    {
        if(!Application.isPlaying) return;
        if(!InputManager.Instance) return;
        
        InputManager.Instance.InputActions.PlayerControls.Walk.performed -= WalkPerformed;
        InputManager.Instance.InputActions.PlayerControls.Walk.canceled -= WalkCancelled;
    }


    private void OnValidate()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

        IsIdling = true;
    }

    private void Update()
    {
        CurrentMove = new Vector2(currentInput.x, currentInput.y) * moveSpeed;
    }

    private void FixedUpdate()
    {
        playerRB.velocity = CurrentMove;
    }

    public void WalkPerformed(InputAction.CallbackContext context)
    {
        currentInput = context.ReadValue<Vector2>();
        lastInput = currentInput;

        IsWalking = true;
        IsIdling = false;
    }

    public void WalkCancelled(InputAction.CallbackContext context)
    {
        currentInput = Vector2.zero;

        IsIdling = true;
        IsWalking = false;

    }
}
