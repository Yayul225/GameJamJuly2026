using System.Collections;
using UnityEngine;

public class RushAttack : EnemyAttack
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject dashVisualPrefab;
    [SerializeField] float dashSpeed = 10f;

    [Header("Damage & Knockback")]
    [SerializeField] float damage = 10f;
    [SerializeField] private float knockbackForce = 12f;

    [Header("Attack Timing")]
    [SerializeField] float dashChargeTime = 1.5f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashRecoveryTime = 1f;

    bool isAttacking;
    private bool canDealDamage;
    private Enemy enemy;
    


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void Execute()
    {
        if (isAttacking)
            return;
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        isAttacking = true;
        enemy.SetBusy(true);
        

        // Dash carga
        enemy.StopMoving();
        yield return new WaitForSeconds(dashChargeTime); // esperamos mientras carga el dash

        // aqui desactivamos la animacion de carga y activamos la animacion de dash

        //Guardar la direccion
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            enemy.AimAt(player.transform.position);
            enemy.FaceTarget(player.transform.position);
        }

        Vector2 dashDirection = firePoint.right.normalized;

        //Parte del Rush o Dash
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        rb.linearVelocity = dashDirection * dashSpeed;

        canDealDamage = true;
        

        yield return new WaitForSeconds(dashDuration); // esperamos mientras dura el dash

        //Aqui desactivamos la animacion de dash y activamos la animacion de finalizacion del dash

        //Parte de Stop del Rush o Dash
        canDealDamage = false;
        enemy.StopMoving();

        //Recuperacion del enemigo despues del dash
        //Aqui activamos la animacion de recuperacion del dash y los efectos visuales de recuperacion
        yield return new WaitForSeconds(dashRecoveryTime); // esperamos mientras dura la recuperacion del dash
        //Aqui desactivamos la animacion de recuperacion del dash y volvemos a la animacion de idle o movimiento normal

        enemy.SetBusy(false);
        isAttacking = false;

    }


    
    // Detectar colision con el Jugador Durante el dash
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!canDealDamage) return;

        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                // Calculate direction pointing FROM enemy TO player
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;

                // Deal damage + knockback
                playerHealth.TakeDamage(damage, knockbackDir, knockbackForce);

                // Disable damage for remainder of this dash so it doesn't hit multiple times
                canDealDamage = false;
            }
        }
    }
}
