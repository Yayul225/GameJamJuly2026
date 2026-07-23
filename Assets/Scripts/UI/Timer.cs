using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    [Header("Configuración del Temporizador")]
    public int durationProgressBar = 30; // Tiempo que tarda la barra en dar una vuelta completa (ej. 30 seg)
    public int countNumber = 10;           // Número desde el que empieza la cuenta regresiva (ej. 10)

    private int remainingDuration;        // Tiempo total acumulado en segundos

    private void Start()
    {
        // El tiempo total es la cantidad de rondas por la duración de cada ronda
        int totalDuration = countNumber * durationProgressBar;
        Begin(totalDuration);
    }

    private void Begin(int second)
    {
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration > 0)
        {
            // 1. Calculamos el número actual del contador (de countNumber hacia 1)
            // Es vital dividir entre (float) para obtener decimales antes de redondear hacia arriba
            int currentCount = Mathf.CeilToInt((float)remainingDuration / durationProgressBar);
            uiText.text = currentCount.ToString();

            // 2. Calculamos los segundos restantes dentro del ciclo actual de la barra (de 30 a 1)
            int cicloBar = remainingDuration % durationProgressBar;

            if (cicloBar == 0 && remainingDuration > 0)
            {
                cicloBar = durationProgressBar;
            }

            // 3. Actualizamos el rellenado de la barra (de 1.0 a 0.0)
            uiFill.fillAmount = Mathf.InverseLerp(0, durationProgressBar, cicloBar);

            yield return new WaitForSeconds(1f);
            remainingDuration--;
        }

        OnEnd();
    }

    private void OnEnd()
    {
        uiText.text = "0";
        uiFill.fillAmount = 0;
        Debug.Log("Finalizado");
    }
}