using System;
using System.Collections;
using UnityEngine;

public class ShootAttack : EnemyAttack
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float damage = 10f;

    [Header("Attack Timing")]
    [SerializeField] private float chargeTime = 0.5f;
    [SerializeField] private float recoveryTime = 1f;

    private bool isAttacking;
    private Enemy enemy;


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public override void Execute()
    {
        if (isAttacking)
            return;
        StartCoroutine(ShootRoutine());
        
    }
    private IEnumerator ShootRoutine()
    {
        isAttacking = true;
        enemy.SetBusy(true);

        enemy.StopMoving();

        yield return new WaitForSeconds(chargeTime);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            enemy.AimAt(player.transform.position);
            enemy.FaceTarget(player.transform.position);
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet.TryGetComponent<Bullet>(out Bullet bulletScript))
        {
            bulletScript.Initialize(firePoint.right, bulletSpeed, damage);
        }

        // Fase de recuperacion
        yield return new WaitForSeconds(recoveryTime);

        enemy.SetBusy(false);
        isAttacking = false;
    }
}
