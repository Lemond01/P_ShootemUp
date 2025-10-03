using System.Collections;
using UnityEngine;
using TMPro; // ¡Importante! Agregar esta línea

public class CrawlText : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI titleText;    // Cambiado a TextMeshProUGUI
    public TextMeshProUGUI subtitleText; // Cambiado a TextMeshProUGUI
    
    [Header("Configuración Animación")]
    public float crawlSpeed = 50f;
    public float startDelay = 2f;
    public float textSpacing = 200f;
    public float endYPosition = 1500f;
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;
    
    [Header("Configuración Pantalla")]
    public float screenHeight = 1080f;

    private void Start()
    {
        // Ocultar textos inicialmente
        if (titleText != null) 
        {
            titleText.gameObject.SetActive(false);
            SetTextAlpha(titleText, 0f);
        }
        
        if (subtitleText != null)
        {
            subtitleText.gameObject.SetActive(false);
            SetTextAlpha(subtitleText, 0f);
        }
        
        StartCoroutine(StartCrawlAnimation());
    }

    private IEnumerator StartCrawlAnimation()
    {
        yield return new WaitForSeconds(startDelay);
        
        Vector3 startPosition = new Vector3(0, -screenHeight, 0);
        
        // Configurar y animar título
        if (titleText != null)
        {
            titleText.rectTransform.anchoredPosition = startPosition;
            titleText.gameObject.SetActive(true);
            yield return StartCoroutine(FadeInText(titleText, fadeInDuration));
        }
        
        // Pequeño delay antes del subtítulo
        yield return new WaitForSeconds(0.5f);
        
        // Configurar y animar subtítulo
        if (subtitleText != null)
        {
            subtitleText.rectTransform.anchoredPosition = startPosition - new Vector3(0, textSpacing, 0);
            subtitleText.gameObject.SetActive(true);
            yield return StartCoroutine(FadeInText(subtitleText, fadeInDuration));
        }
        
        // Iniciar movimiento
        yield return StartCoroutine(AnimateTexts());
    }

    private IEnumerator AnimateTexts()
    {
        bool titleFinished = false;
        bool subtitleFinished = false;

        while (!titleFinished || !subtitleFinished)
        {
            // Mover título
            if (titleText != null && !titleFinished)
            {
                titleText.rectTransform.anchoredPosition += Vector2.up * crawlSpeed * Time.deltaTime;
                
                if (titleText.rectTransform.anchoredPosition.y >= endYPosition - 500f)
                {
                    yield return StartCoroutine(FadeOutText(titleText, fadeOutDuration));
                    titleFinished = true;
                    titleText.gameObject.SetActive(false);
                }
            }

            // Mover subtítulo
            if (subtitleText != null && !subtitleFinished)
            {
                subtitleText.rectTransform.anchoredPosition += Vector2.up * crawlSpeed * Time.deltaTime;
                
                if (subtitleText.rectTransform.anchoredPosition.y >= endYPosition - 500f)
                {
                    yield return StartCoroutine(FadeOutText(subtitleText, fadeOutDuration));
                    subtitleFinished = true;
                    subtitleText.gameObject.SetActive(false);
                }
            }

            yield return null;
        }
    }

    // Efecto de fade in para texto
    private IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            SetTextAlpha(text, alpha);
            yield return null;
        }
        SetTextAlpha(text, 1f);
    }

    // Efecto de fade out para texto
    private IEnumerator FadeOutText(TextMeshProUGUI text, float duration)
    {
        float elapsed = 0f;
        float startAlpha = text.color.a;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            SetTextAlpha(text, alpha);
            yield return null;
        }
        SetTextAlpha(text, 0f);
    }

    // Cambiar transparencia del texto (sobrecarga para TextMeshProUGUI)
    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}