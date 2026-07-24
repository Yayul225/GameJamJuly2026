using System.Collections;
using UnityEngine;

public class RushAttack : EnemyAttack
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject dashVisualPrefab;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashChargeTime = 1.5f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashRecoveryTime = 1f;
    [SerializeField] float damage = 10f;
    bool isAttacking;
    private Enemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


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
        Debug.Log("Rush Attack Started");

        // Dash carga
        enemy.StopMoving();
        //Aqui puedo poner animaciones y efectos visuales de carga del dash

        yield return new WaitForSeconds(dashChargeTime); // esperamos mientras carga el dash

        // aqui desactivamos la animacion de carga y activamos la animacion de dash

        //Guardar la direccion
        Vector2 dashDirection = firePoint.right.normalized;

        //Parte del Rush o Dash

        Debug.Log("Dashing");
        rb.linearVelocity = dashDirection * dashSpeed; // CHECK IF THIS IS CORRECT
        //aqui activamos la animacion de dash y los efectos visuales del dash

        yield return new WaitForSeconds(dashDuration); // esperamos mientras dura el dash

        //Aqui desactivamos la animacion de dash y activamos la animacion de finalizacion del dash

        //Parte de Stop del Rush o Dash
        enemy.StopMoving();

        //Recuperacion del enemigo despues del dash
        //Aqui activamos la animacion de recuperacion del dash y los efectos visuales de recuperacion
        yield return new WaitForSeconds(dashRecoveryTime); // esperamos mientras dura la recuperacion del dash
        //Aqui desactivamos la animacion de recuperacion del dash y volvemos a la animacion de idle o movimiento normal

        enemy.SetBusy(false);
        isAttacking = false;

    }
}
