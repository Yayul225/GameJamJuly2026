using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float damage;
    [SerializeField] private float knockbackForce = 4f;

    [SerializeField] private float lifeTime = 5f; // Tiempo de vida del proyectil

    private bool hasHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed, float bulletDamage)
    {
        damage = bulletDamage;
        rb.linearVelocity = direction.normalized * speed;


        Destroy(gameObject, lifeTime);

    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;

            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                // Calcular direccion de la bala al jugador
                Vector2 knockbackDir = rb.linearVelocity.normalized;

                // Deal damage + knockback
                playerHealth.TakeDamage(damage, knockbackDir, knockbackForce);
            }

            Destroy(gameObject); // Se destruye al impactar
        }
        else if (other.CompareTag("Obstacle"))
        {
            hasHit = true;
            Destroy(this.gameObject);
        }
    }
}
