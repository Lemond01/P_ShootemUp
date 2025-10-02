using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip buttonSound;
    public AudioSource audioSource;
    
    [Header("Scene Settings")]
    public string sceneName; // Nombre de la escena a cargar
    public int sceneIndex = -1; // Índice de la escena a cargar (-1 para usar nombre)
    
    public void ChangeScene()
    {
        StartCoroutine(ChangeSceneWithSound());
    }

    private IEnumerator ChangeSceneWithSound()
    {
        // Reproducir sonido si existe
        if (buttonSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonSound);
            // Esperar a que termine el sonido
            yield return new WaitForSeconds(buttonSound.length);
        }
        
        // Cambiar a la escena destino
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else if (sceneIndex >= 0)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("No se ha especificado una escena válida para cargar");
        }
    }
}