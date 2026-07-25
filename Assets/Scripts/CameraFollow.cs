using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Arrastrar jugador aqui
    [SerializeField] private float smoothTime = 0.2f; // tiempo para alcnazar al jugadro==or
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // mantiene Z a -10 para 2D

    private Vector3 currentVelocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        // Desired position using player's position + offset
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera toward the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
