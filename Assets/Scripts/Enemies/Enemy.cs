using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    //VARIABLES DE MOVIEMIENTO
    [SerializeField] public float detectRadius = 5f; //la IA leera este valor
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float stopDistance = 1f;
    [SerializeField] float patrolRadius = 5f;

    //VARIABLES DE ATAQUE
    [SerializeField] public float attackRadius = 1f; //la ia leera este valor
    [SerializeField] float attackCoolDown = 1f;
    [SerializeField] float attackDamage = 25f;
    private float lastAttackTime = 0f;

    bool isDead = false;
    [SerializeField] float health = 100f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
