using UnityEngine;

public class ItemPickupHandler : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform pickUpPoint;

    [Header("Configuración de Tags")]
    [SerializeField] private string itemTag = "Item";
    [SerializeField] private string dropPointTag = "DropPoint";

    private PlayerAttack playerAttack;
    private Animator anim;
    private GameObject currentItem;
    private bool isHoldingItem = false;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // CASO 1: Recoger ítem
        if (!isHoldingItem && other.CompareTag(itemTag))
        {
            bool isAttacking = anim != null && anim.GetBool("isAttacking");

            if (isAttacking)
            {
                PickUpItem(other.gameObject);
            }
        }
        // CASO 2: Entregar en DropPoint
        else if (isHoldingItem && other.CompareTag(dropPointTag))
        {
            // Verificamos si el DropPoint está libre
            DropPoint dropPoint = other.GetComponent<DropPoint>();
            if (dropPoint != null && dropPoint.IsAvailable())
            {
                DropItem(dropPoint);
            }
        }
    }

    private void PickUpItem(GameObject item)
    {
        isHoldingItem = true;
        currentItem = item;

        if (playerAttack != null)
        {
            playerAttack.enabled = false;
        }

        Rigidbody2D itemRb = currentItem.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            itemRb.simulated = false;
        }

        currentItem.transform.SetParent(pickUpPoint);
        currentItem.transform.localPosition = Vector3.zero;

        ItemDropDetector detector = currentItem.GetComponent<ItemDropDetector>();
        if (detector == null)
        {
            detector = currentItem.AddComponent<ItemDropDetector>();
        }
        detector.Initialize(this, dropPointTag);
    }

    public void DropItem(DropPoint dropPoint)
    {
        if (!isHoldingItem || currentItem == null) return;

        // Marcar el DropPoint como ocupado y desactivar su trigger
        dropPoint.Occupy();

        // Eliminar detector del ítem
        ItemDropDetector detector = currentItem.GetComponent<ItemDropDetector>();
        if (detector != null)
        {
            Destroy(detector);
        }

        // Mover el ítem al DropPoint
        currentItem.transform.SetParent(dropPoint.transform);
        currentItem.transform.localPosition = Vector3.zero;

        // Reactivar físicas del ítem
        Rigidbody2D itemRb = currentItem.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            itemRb.simulated = true;
        }

        // Limpiar referencias del jugador
        currentItem = null;
        isHoldingItem = false;

        // Reactivar el ataque del jugador
        if (playerAttack != null)
        {
            playerAttack.enabled = true;
        }
    }
}