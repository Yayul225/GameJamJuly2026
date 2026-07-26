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
    bool isAttacking = false;
    float punchTimer = 0f;

    [Header("Item Carrying Settings")]
    [SerializeField] private Transform pickupPoint;

    bool isCarryingItem;
    private bool isNearDropPoint;
    
    private GameObject carriedItem;

    [SerializeField] private float dropRadius = 2f;
    [SerializeField] private LayerMask dropPointLayer; // Or check Tag directly


    // Update is called once per frame
    void Update()
    {
        if (punchTimer > 0f)
        {
            punchTimer -= Time.deltaTime;
        }

        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Debug.Log(context.phase);

        if (isCarryingItem)
        {
            IsNearDropPoint();



            return; // ⛔ Cannot attack while holding an item
        }

        if (punchTimer <= 0f)
        {
            Debug.Log("Estoy DENTRO del ataque");
                PerformPunch();
            
        }
    }


    private void PerformPunch()
    {
        //Alternar lado de ataque
        attackRight = !attackRight;

        //cambiar parametros de animator
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        anim.SetBool("isAttackingRight", attackRight);

        //reiniciar el cooldown de ataque
        punchTimer = punchCoolDown;

        //Parar ataque despues de un tiempo
        Invoke(nameof(StopAttack), 0.3f);
    }
    void StopAttack()
    {
        anim.SetBool("isAttacking", false);
        isAttacking = false;
    }

    private void PickupItem(GameObject item)
    {
        carriedItem = item;
        isCarryingItem = true;
        

        // Attach item to player's pickup point
        carriedItem.transform.SetParent(pickupPoint);
        carriedItem.transform.localPosition = Vector3.zero;

        // Disable physics/colliders while carrying
        if (carriedItem.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) rb.simulated = false;
        if (carriedItem.TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;

        Debug.Log($"Picked up: {carriedItem.name}");
    }

    private void IsNearDropPoint()
    {
        // Check if any collider on dropPointLayer is inside the circle radius around the player
        Collider2D hit = Physics2D.OverlapCircle(transform.position, dropRadius, dropPointLayer);
        

        if (hit != null && hit.CompareTag("DropPoint"))
        {
            // ⚠️ Compare item name to drop point name (e.g. "Carrot" matches "CarrotDropPoint")
            if (hit.gameObject.name.Contains(carriedItem.name.Replace("(Clone)", "").Trim()))
            {
                carriedItem.transform.SetParent(hit.gameObject.transform);
                carriedItem.transform.localPosition = Vector3.zero;
                carriedItem.transform.rotation = hit.transform.rotation;

                carriedItem = null; // Reset reference
                isCarryingItem = false;

                Debug.Log("Item delivered to the correct spot!");

                // 🏆 NOTIFY GAME MANAGER OF DELIVERY
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ItemDelivered();
                }
            }
            else
            {
                Debug.Log("Wrong drop point for this item!");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!isAttacking) return;

        if (other.CompareTag("Enemy"))
        {
            //ACTIVALO DESPUES CUANDO ESTEN LOS ENEMIGOS
            other.GetComponent<Enemy>().TakeDamage(punchDamage);
            Debug.Log("enemy hit");
        }
        else if (other.CompareTag("Item"))
        {
            PickupItem(other.gameObject);
        }
        
        
    }
}
