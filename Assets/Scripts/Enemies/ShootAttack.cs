using UnityEngine;

public class ShootAttack : EnemyAttack
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float damage = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    public override void Execute()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if(bulletScript != null)
        {
            bulletScript.Initialize(firePoint.right, bulletSpeed, damage);
        }
    }
}
