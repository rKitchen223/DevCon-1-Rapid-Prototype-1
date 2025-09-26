/*
    Ethan Gapic-Kott, 000923124
*/

/// Script used to add a death panel after falling into the void or "DeathBox"

using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBox : MonoBehaviour
{
    public GameObject deathPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f; // Freeze game on player death
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
