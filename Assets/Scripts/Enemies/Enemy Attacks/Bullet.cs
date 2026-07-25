using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float damage;

    [SerializeField] private float lifeTime = 5f; // Tiempo de vida del proyectil

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
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Se destruye al impactar
        }
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject);
        }
    }
}
