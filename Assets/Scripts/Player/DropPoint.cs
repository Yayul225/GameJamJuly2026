using UnityEngine;

public class DropPoint : MonoBehaviour
{
    [Header("Estado")]
    public bool isOccupied = false;

    // Métodos para verificar y cambiar estado
    public bool IsAvailable()
    {
        return !isOccupied;
    }

    public void Occupy()
    {
        isOccupied = true;

        // Opcional: Desactivar el Collider para que ni el jugador ni otros ítems
        // vuelvan a disparar eventos con este DropPoint
        /*Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Vacate();
        }
    }

    public void Vacate()
    {
        isOccupied = false;
        Debug.Log($"El DropPoint{gameObject.name} vuelve a estar disponible.");
    }
}