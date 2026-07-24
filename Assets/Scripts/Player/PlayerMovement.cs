using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float moveSpeed = 5f;
    private Vector2 moveDir;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Obtenemos el componente Rigidbody2D del objeto al que está adjunto este script
        rb = GetComponent<Rigidbody2D>();
    }

    
    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Obtenemos la dirección de movimiento a partir del input del jugador
        moveDir = context.ReadValue<Vector2>().normalized;
    }

    private void Move()
    {
        //movernos con fisicas
        //si no hay movimiento en ninguna dirección, establecemos la velocidad lineal del Rigidbody2D a cero
        if (moveDir == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
        //si hay movimiento, establecemos la velocidad lineal del Rigidbody2D en la dirección de movimiento multiplicada por la velocidad de movimiento
        else
        {
            rb.linearVelocity = moveDir * moveSpeed;
        }
    }
}
