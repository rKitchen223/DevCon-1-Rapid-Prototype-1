/*
    Ethan Gapic-Kott, 000923124
*/

/// Script used to add a win panel after interacting with the "WinBox"
/// Copied the DeathBox Script as it has the exact same functionality, Just displays different text

using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBox : MonoBehaviour
{
    public GameObject deathPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f; // Freeze game on player win
        }
    }

    // Button functions for restarting or quitting the application
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
