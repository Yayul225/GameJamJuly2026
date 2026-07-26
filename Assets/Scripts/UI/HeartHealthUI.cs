using UnityEngine;
using UnityEngine.UI;

public class HeartHealthUI : MonoBehaviour
{
    [Header("Heart UI Elements")]
    [SerializeField] private Image[] heartImages; // Drag your 5 Heart Image objects here in order (0 to 4)

    [Header("Heart Colors")]
    [SerializeField] private Color fullHeartColor = Color.red;     // Bright red
    [SerializeField] private Color emptyHeartColor = Color.black;   // Black / Dark empty color

    /// <summary>
    /// Call this whenever player health changes (currentHealth between 0 and 5).
    /// </summary>
    public void UpdateHearts(float currentHealth)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null) continue;

            // If the heart index is less than current health, paint it Red.
            // Example: currentHealth = 3 -> Index 0, 1, 2 are Red; Index 3, 4 are Black.
            if (i < currentHealth)
            {
                heartImages[i].color = fullHeartColor;
            }
            else
            {
                heartImages[i].color = emptyHeartColor;
            }
        }
    }
}
