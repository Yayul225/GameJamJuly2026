using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuScript : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject ButtonOpenMenu; // El botón UI para pausar (opcional)
    public GameObject PausePanel;     // El panel completo del menú de pausa

    private bool isPaused = false;

    private void Update()
    {
        // Al presionar Escape, conmuta el estado (Abre o Cierra)
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame(); // Se reanuda si ya estaba pausado
            }
            else
            {
                PauseGame();  // Se pausa si estaba en juego normal
            }
        }
    }

    // Se llama al presionar ESC o al hacer clic en el botón de Pausa en pantalla
    public void PauseGame()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);

        if (ButtonOpenMenu != null)
            ButtonOpenMenu.SetActive(false);

        isPaused = true;
    }

    // Se llama al presionar ESC o al hacer clic en el botón "Reanudar"
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);

        if (ButtonOpenMenu != null)
            ButtonOpenMenu.SetActive(true);

        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Restaurar el tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}