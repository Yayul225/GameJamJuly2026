using UnityEngine;
using UnityEngine.UI;
using TMPro; //libreria necesaria para trabajar con TextMeshPro

public class ScriptPrueba : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshProUGUI textCountNum; //Aqui arrastramos el texto del conteo

    [Header("Configuración")]
    [SerializeField] private int limitNumChange = 1; //Número designado para cuando se quiera que ocurra el evento
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
