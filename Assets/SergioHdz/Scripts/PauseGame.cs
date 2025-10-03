using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [Header("Pause Menu UI")]
    public GameObject pauseMenuUI;
    public bool isPaused = false;

    [Header("Audio Settings")]
    public AudioClip resumeSound;
    public AudioClip menuSound;
    public AudioSource audioSource;

    [Header("Scene Settings")]
    [Tooltip("Nombre de la escena del menú principal")]
    public string menuSceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ReturnGame();
            }
            else
            {
                PausedGame();
            }
        }
    }

    public void ReturnGame()
    {
        StartCoroutine(ReturnGameWithSound());
    }

    public void PausedGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ReturnMenu()
    {
        StartCoroutine(ReturnMenuWithSound());
    }

    private IEnumerator ReturnGameWithSound()
    {
        if (resumeSound != null && audioSource != null)
        {
            // como el juego está pausado con Time.timeScale = 0f
            // usamos WaitForSecondsRealtime para que sí avance el tiempo real
            audioSource.PlayOneShot(resumeSound);
            yield return new WaitForSecondsRealtime(resumeSound.length);
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private IEnumerator ReturnMenuWithSound()
    {
        if (menuSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(menuSound);
            yield return new WaitForSecondsRealtime(menuSound.length);
        }

        // restauramos el tiempo antes de cambiar de escena
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
        else
        {
            Debug.LogError("⚠️ No se ha asignado el nombre de la escena del menú en el inspector.");
        }
    }
}

