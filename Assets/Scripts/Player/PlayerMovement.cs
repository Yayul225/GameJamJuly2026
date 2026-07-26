using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    private PlayerHealth playerHealth;

    [SerializeField] float moveSpeed = 5f;
    private Vector2 moveDir;




    void Awake()
    {
        // Use Awake for GetComponent to ensure references exist BEFORE scene updates
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnDisable()
    {
        // Reset move direction if player is disabled/reloaded
        moveDir = Vector2.zero;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Read input during performed and canceled phases
        if (context.performed || context.canceled)
        {
            moveDir = context.ReadValue<Vector2>().normalized;
        }
    }

    private void Move()
    {
        if (playerHealth != null && playerHealth.IsKnockedBack())
        {
            return; // Allow knockback physics to process uninterrupted
        }

        rb.linearVelocity = moveDir * moveSpeed;
    }
}
