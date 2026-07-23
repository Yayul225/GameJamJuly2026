using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject ButtonOpenMenu;
    public GameObject PausePanel;
    public void PauseGame()
    {
        Time.timeScale = 0;
        ButtonOpenMenu.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        ButtonOpenMenu.SetActive(true);
        PausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
