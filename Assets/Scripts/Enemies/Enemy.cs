using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    //VARIABLES DE MOVIEMIENTO
    [SerializeField] public float detectRadius = 5f; //la IA leera este valor
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float stopDistance = 1f;
    [SerializeField] float patrolRadius = 5f;
    [SerializeField] Transform spritePivot; //punto de pivote del sprite para rotarlo hacia el jugador


    //VARIABLES DE ATAQUE
    [SerializeField] private EnemyAttack attack;
    [SerializeField] public float attackRadius = 1f; //la ia leera este valor
    [SerializeField] float attackCoolDown = 1f;
    [SerializeField] float attackDamage = 25f; //TALVEZ QUITARLO SI NO SE USA EN EL ATAQUE
    private float lastAttackTime = 0f;
    [SerializeField] Transform firePivot;


    bool isDead = false;
    [SerializeField] float health = 100f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attack = GetComponent<EnemyAttack>();
    }

    public void MoveTo(Vector2 targetPos)
    {
        Vector2 currentPos = rb.position; //guardamos posicion actual del enemigo
        Vector2 direction = (targetPos - currentPos).normalized; //calculamos la direccion hacia el objetivo

        float distanceToTarget = Vector2.Distance(currentPos, targetPos); //calculamos la distancia al objetivo

        

        rb.linearVelocity = direction * moveSpeed;
        FaceTarget(targetPos);
    }

    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public Vector2 GetRandomPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius; //generamos un vector aleatorio dentro de un circulo
        return (Vector2)transform.position + randomOffset; //retornamos la posicion aleatoria sumada a la posicion actual del enemigo
    }

    public bool CanAttack()
    {
        //solo revisamos si el tiempo de enfriamiento ha pasado
        return Time.time >= lastAttackTime + attackCoolDown;
    }

    public void TryAttack()
    {
        if (CanAttack())
            Attack();
    }

    public void Attack()
    {
        lastAttackTime = Time.time;//actualizamos el tiempo del ultimo ataque
        //PLACEHOLDER: Aqui iria la logica de ataque, como por ejemplo restar vida al jugador
        attack.Execute();
    }

    public void TakeDamage(float damage)
    {
        health -= damage; //reducimos salud
        Debug.Log($"{name} took {damage} damage. Remaining health: {health}"); //debug message para mostrar salud

        if (health <= 0) //si salud llega a cero o menos, el enemigo muere
            Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        StopMoving();
        Debug.Log($"{name} has died!");
        Destroy(gameObject);
    }

    public void FaceTarget(Vector2 targetPos)
    {
        Vector3 scale = spritePivot.localScale;
        if (targetPos.x < transform.position.x && scale.x > 0)
            scale.x *= -1;
        else if (targetPos.x > transform.position.x && scale.x < 0)
            scale.x *= -1;

        spritePivot.localScale = scale;
    }

    public void AimAt(Vector2 target)
    {
        //calculamos la rotacion necesaria para apuntar al objetivo
        Vector2 direction = target - (Vector2)transform.position;

        //calculamos el angulo en grados usando Atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //aplicamos la rotacion al pivote de disparo
        firePivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    // El Gizmos es perfecto, ahora usará los valores de este script
    // que la IA también leerá.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
