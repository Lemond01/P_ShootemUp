using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip playSound;
    public AudioClip exitSound;
    public AudioSource audioSource;

    [Header("Scene Settings")]
    [Tooltip("Nombre de la escena del juego que quieres cargar")]
    public string gameSceneName;

    public void PlayGame()
    {
        StartCoroutine(PlayGameWithSound());
    }

    public void Exit()
    {
        StartCoroutine(ExitWithSound());
    }

    private IEnumerator PlayGameWithSound()
    {
        if (playSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(playSound);
            yield return new WaitForSeconds(playSound.length);
        }

        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("⚠️ No se ha asignado el nombre de la escena en el inspector.");
        }
    }

    private IEnumerator ExitWithSound()
    {
        if (exitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(exitSound);
            yield return new WaitForSeconds(exitSound.length);
        }

        Debug.Log("Exit Game...");
        Application.Quit();
    }
}

