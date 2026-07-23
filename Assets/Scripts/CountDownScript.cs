using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CountDownScript : MonoBehaviour//, IPointerClickHandler
{
    /*public void OnPointerClick(PointerEventData eventData)
    {
        Pause = !Pause;
    }*/

    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    public int Duration; // Duración total en segundos

    private int remainingDuration;
    //private bool Pause;

    private void Start()
    {
        Begin(Duration);
    }

    private void Begin(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            // 1. Muestra solo el minuto actual (ej: "10", "9", "8"...)
            // Usamos Mathf.CeilToInt para que muestre el minuto actual de forma redondeada hacia arriba
            int currentMinute = Mathf.CeilToInt(remainingDuration / 60f);
            uiText.text = currentMinute.ToString();

            // 2. Calcula los segundos actuales dentro del ciclo de 1 minuto (0 a 59)
            int secondsInCurrentMinute = remainingDuration % 60;

            // Si los segundos dan 0 pero aún queda tiempo, la rueda debe estar llena (60s)
            if (secondsInCurrentMinute == 0 && remainingDuration > 0)
            {
                secondsInCurrentMinute = 60;
            }

            // 3. Rellena la rueda en un rango de 0 a 60 segundos
            uiFill.fillAmount = Mathf.InverseLerp(0, 60, secondsInCurrentMinute);

            remainingDuration--;
            yield return new WaitForSeconds(1f);
            /*if (!Pause)
            {
               
            }
            yield return null;*/
        }

        OnEnd();
    }

    private void OnEnd()
    {
        uiText.text = "0";
        uiFill.fillAmount = 0;
        print("End");
    }
}
