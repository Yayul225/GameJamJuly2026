using UnityEngine;
using UnityEngine.InputSystem;

public class PointToMouse : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    //moise point support
    [SerializeField] private InputActionReference pointAction;
    Vector2 mousePos;
    private Transform playerTransform;
    private float angle;
    [SerializeField] float angleOffset = 0f;

    [SerializeField]
    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // convertir el la posicion del mouse a la posicion del mundo
        //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos = cam.ScreenToWorldPoint(pointAction.action.ReadValue<Vector2>());


        // calculamos el vector de direccion entre el jugador y el mouse
        Vector2 lookDir = mousePos - (Vector2)playerTransform.position;

        // calculamos el angulo en radianes entre el vector de direccion y el eje x
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - angleOffset;

        // aplicamos la rotacion al objeto padre de los Fists
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

   

}
