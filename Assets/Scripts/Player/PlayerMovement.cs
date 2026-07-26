using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;
    private PlayerHealth playerHealth;

    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 5f;
    private Vector2 moveDir;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        if (playerInput != null)
        {
            playerInput.currentActionMap?.Enable();
        }
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.currentActionMap?.Enable();
        }
    }

    private void OnDisable()
    {
        moveDir = Vector2.zero;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            Debug.Log("MOviento fase:" + context.phase);
            moveDir = context.ReadValue<Vector2>().normalized;
        }
    }

    private void Move()
    {
        if (playerHealth != null && playerHealth.IsKnockedBack())
        {
            return; // Allow knockback physics to process uninterrupted
        }

        rb.linearVelocity = moveDir * playerSpeed;
    }

    public void ApplySpeedBoost(float multiplier)
    {
        playerSpeed *= multiplier;
        Debug.Log($"Speed boosted! New playerSpeed: {playerSpeed}");
    }
}