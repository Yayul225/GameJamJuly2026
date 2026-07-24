using UnityEngine;

public class ItemDropDetector : MonoBehaviour
{
    private ItemPickupHandler handler;
    private string dropTag;

    public void Initialize(ItemPickupHandler pickupHandler, string targetTag)
    {
        handler = pickupHandler;
        dropTag = targetTag;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(dropTag))
        {
            DropPoint dropPoint = other.GetComponent<DropPoint>();
            if (dropPoint != null && dropPoint.IsAvailable())
            {
                handler.DropItem(dropPoint);
            }
        }
    }
}