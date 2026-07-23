using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptPrueba : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshProUGUI textCountNum;

    [Header("Configuración")]
    [SerializeField] private int limitNumChange = 1;
    [SerializeField] private Color colorNew = Color.green;

    private SpriteRenderer spriteRenderer;
    private bool isColorChange = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (textCountNum == null || isColorChange) return;

        // convertimos el texto del TMP a un numero entero
        if (int.TryParse(textCountNum.text, out int currentValue))
        {
            if (currentValue <= limitNumChange )
            {
                spriteRenderer.color = colorNew;
                isColorChange = true;
            }
        }
    }
}
