using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    //VARIABLES DE MOVIEMIENTO
    [SerializeField] public float detectRadius = 5f; //la IA leera este valor
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float patrolRadius = 5f;
    [SerializeField] Transform spritePivot; //punto de pivote del sprite para rotarlo hacia el jugador

    [Header("Patrol Zone Settings")]
    [SerializeField] private Vector2 patrolAreaCenter;
    [SerializeField] private Vector2 patrolAreaSize;

    //VARIABLES DE ATAQUE
    [SerializeField] private EnemyAttack attack;
    [SerializeField] public float attackRadius = 1f; //la ia leera este valor
    [SerializeField] float attackCoolDown = 1f;
    [SerializeField] float attackDamage = 25f; //TALVEZ QUITARLO SI NO SE USA EN EL ATAQUE
    private float lastAttackTime = 0f;
    [SerializeField] Transform firePivot;


    //VARIABLES DE DETECCION
    [SerializeField] private LayerMask obstacleMask;
    public bool IsBusy { get; private set; }


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
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Vector2 currentPos = rb.position; //guardamos posicion actual del enemigo
        Vector2 direction = (targetPos - currentPos).normalized; //calculamos la direccion hacia el objetivo

        float distanceToTarget = Vector2.Distance(currentPos, targetPos); //calculamos la distancia al objetivo

        

        rb.linearVelocity = direction * moveSpeed;
        FaceTarget(targetPos);
    }

    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public Vector2 GetRandomPosition()
    {
        Vector2 randomPoint = Vector2.zero;
        bool validPointFound = false;
        int maxAttempts = 20;

        for(int i = 0; i < maxAttempts; i++)
        {
            //Calcular un punto aleatorio dentro del área de patrulla
            float randomX = Random.Range(patrolAreaCenter.x - patrolAreaSize.x / 2, patrolAreaCenter.x + patrolAreaSize.x / 2);
            float randomY = Random.Range(patrolAreaCenter.y - patrolAreaSize.y / 2, patrolAreaCenter.y + patrolAreaSize.y / 2);
            Vector2 candidatePoint = new Vector2(randomX, randomY);

            //Verificamos que el punto esten el rango de vision del enemigo y que el punto no esta dentro de un obstaculo
            if(HasLineOffSightTo(candidatePoint) && !Physics2D.OverlapPoint(candidatePoint, obstacleMask))
            {
                randomPoint = candidatePoint;
                validPointFound = true;
                break;
            }
        }
        if(!validPointFound)
        {
            //Si no se encuentra un punto valido, se devuelve la posicion actual del enemigo
            randomPoint = transform.position;
        }
        return randomPoint;
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

        StopAllCoroutines(); //detenemos todas las corutinas que pueda estar ejecutando el enemigo
        StopMoving(); //detenemos el movimiento del enemigo

        if (TryGetComponent<Collider2D>(out Collider2D col))
        {
            col.enabled = false; // Prevents dead enemy from taking hits or blocking player
        }
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

    public bool HasLineOffSightTo(Vector2 targetPos)
    {
        Vector2 origin = transform.position;
        Vector2 direction = targetPos - origin;
        float distance = direction.magnitude;

        //lanza un raycast para verificar si hay un obstaculo entre el enemigo y el objetivo
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, obstacleMask);
        return hit.collider == null;
    }

    public void SetBusy(bool busy)
    {
        IsBusy = busy;
    }

    public void SetPatrolZone(Vector2 center, Vector2 size)
    {
        patrolAreaCenter = center;
        patrolAreaSize = size;
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
