using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [Header("Attack Settings")]
    [SerializeField] private float  punchCoolDown = 0.4f;
    [SerializeField] private float punchDamage = 50f;
    bool attackRight = false;
    float punchTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);

        if(context.performed  && punchTimer <= 0f)
        {
            //Alternar lado de ataque
            attackRight = !attackRight;

            //cambiar parametros de animator
            anim.SetBool("isAttacking", true);
            anim.SetBool("isAttackingRight", attackRight);

            //reiniciar el cooldown de ataque
            punchTimer = punchCoolDown;

            //Parar ataque despues de un tiempo
            Invoke(nameof(StopAttack), 0.3f);
        }
    }

    void StopAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //ACTIVALO DESPUES CUANDO ESTEN LOS ENEMIGOS
            //other.GetComponent<Enemy>().TakeDamage(punchDamage);
            //Debug.Log("enemy hit");
        }
    }
}
