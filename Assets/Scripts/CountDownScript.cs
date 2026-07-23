using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class CountDownScript : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    [Header("Configuración Principal")]
    public int countNumber = 10;            // Número inicial del contador

    [Header("Tiempos por Fase")]
    public int durationNormal = 60;        // Duración (seg) cuando countNumber > limiteCambio
    public int durationQuick = 30;        // Duración (seg) cuando countNumber <= limiteCambio
    public int limitChange = 5;           // A partir de qué número cambia la velocidad

    private void Start()
    {
        StartCoroutine(StartTimerProcess());
    }

    private IEnumerator StartTimerProcess()
    {
        // Bucle que recorre desde el número inicial (ej. 10) hasta 1
        while (countNumber > 0)
        {
            // 1. Mostramos el número actual en el texto
            uiText.text = countNumber.ToString();

            // 2. Determinamos la duración de la barra para este número en específico
            int currentProgressBarDuration = (countNumber <= limitChange) ? durationQuick : durationNormal;

            // 3. Ejecutamos la animación de la barra para este número
            int remainingSecondsInStep = currentProgressBarDuration;

            while (remainingSecondsInStep > 0)
            {
                // Actualizamos la barra de 1.0 a 0.0
                uiFill.fillAmount = (float)remainingSecondsInStep / currentProgressBarDuration;

                yield return new WaitForSeconds(1f);
                remainingSecondsInStep--;
            }

            // Al terminar la vuelta de la barra, restamos 1 al contador
            countNumber--;
        }

        OnEnd();
    }

    private void OnEnd()
    {
        uiText.text = "0";
        uiFill.fillAmount = 0;
        Debug.Log("Temporizador Finalizado");
    }
}