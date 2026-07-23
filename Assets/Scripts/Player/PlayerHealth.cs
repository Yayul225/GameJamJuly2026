using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    bool isDead = false;
    [SerializeField] float health = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{name} took {damage} damage. Remaining health: {health}");

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        
        Debug.Log($"{name} has died!");
        Destroy(gameObject);
    }
}
