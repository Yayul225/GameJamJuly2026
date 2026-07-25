using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    bool isDead = false;
    [SerializeField] float health = 100f;
    private Rigidbody2D rb;
    private bool isKnockedBack;
    [SerializeField] float knockBackTime = 1.0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        health -= damage;
        Debug.Log($"{name} took {damage} damage. Remaining health: {health}");

        // Aplicar knockback
        if (knockbackForce > 0f)
        {
            StartCoroutine(ApplyKnockbackRoutine(knockbackDirection, knockbackForce));
        }

        if (health <= 0)
            Die();
    }

    private IEnumerator ApplyKnockbackRoutine(Vector2 direction, float force)
    {
        isKnockedBack = true;

        // Reset current velocity & apply impulse force
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        Debug.Log("Emujado hacia" + direction);
        // Wait 0.15s - 0.2s while player is pushed
        yield return new WaitForSeconds(knockBackTime);

        // Reset velocity after knockback finishes
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        
        Debug.Log($"{name} has died!");
        Destroy(gameObject);
    }

    public bool IsKnockedBack() => isKnockedBack;
}
