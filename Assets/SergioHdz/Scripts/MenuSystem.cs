using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip playSound;
    public AudioClip exitSound;
    public AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // Reproducir sonido si existe
        if (playSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(playSound);
            // Esperar a que termine el sonido
            yield return new WaitForSeconds(playSound.length);
        }
        
        // Cargar la siguiente escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator ExitWithSound()
    {
        // Reproducir sonido si existe
        if (exitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(exitSound);
            // Esperar a que termine el sonido
            yield return new WaitForSeconds(exitSound.length);
        }
        
        // Salir del juego
        Debug.Log("Exit Game...");
        Application.Quit();
    }
}
