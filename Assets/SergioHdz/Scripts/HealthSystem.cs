using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Configuration")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Scene Configuration")]
    public string deathSceneName;
    public int deathSceneIndex = -1;
    public float sceneChangeDelay = 3f;

    [Header("Sound Effects")]
    public AudioClip damageSound;
    public AudioClip deathSound;
    private AudioSource audioSource;
    public float soundVolume = 1f;

    [Header("Explosion Effect")]
    public GameObject explosionPrefab;
    public ParticleSystem explosionChild;
    public GameObject shipVisual;

    [Header("Render Settings")]
    public string explosionLayer = "Default";
    public int explosionSortingOrder = 1000;

    [Header("Debug")]
    public KeyCode damageKey = KeyCode.M;
    public int debugDamageAmount = 10;

    // Evento para notificar cuando la nave se oculta
    public System.Action OnShipHidden;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (explosionChild != null)
        {
            explosionChild.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosionChild.gameObject.SetActive(false);
        }

        Debug.Log($"Vida inicializada: {currentHealth}/{maxHealth}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(damageKey) && !isDead)
        {
            TakeDamage(debugDamageAmount);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"Daño recibido: {amount}. Vida restante: {currentHealth}");

        PlayDamageSound();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(DeathSequence());
        }
    }

    private void PlayDamageSound()
    {
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound, soundVolume);
        }
    }

    private IEnumerator DeathSequence()
    {
        isDead = true;
        Debug.Log("¡Has muerto!");

        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound, soundVolume);
            Debug.Log("Sonido de muerte reproducido");
        }

        HideShip();

        // Notificar que la nave se ocultó
        OnShipHidden?.Invoke();
        
        if (explosionChild != null)
        {
            ForceExplosionVisibility(explosionChild.gameObject);
            explosionChild.gameObject.SetActive(true);
            explosionChild.Play(true);
            Debug.Log("Explosion (child) activada y forzada a ser visible");

            while (explosionChild.IsAlive(true))
            {
                yield return null;
            }
        }
        else if (explosionPrefab != null)
        {
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);
            ForceExplosionVisibility(explosionInstance);
            
            ParticleSystem ps = explosionInstance.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.Play(true);
                while (ps.IsAlive(true))
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(sceneChangeDelay);
            }
        }
        else
        {
            yield return new WaitForSeconds(sceneChangeDelay);
        }

        LoadDeathScene();
    }

    private void ForceExplosionVisibility(GameObject explosionObject)
    {
        if (!string.IsNullOrEmpty(explosionLayer))
        {
            explosionObject.layer = LayerMask.NameToLayer(explosionLayer);
            SetLayerRecursively(explosionObject, LayerMask.NameToLayer(explosionLayer));
        }

        ParticleSystemRenderer[] psRenderers = explosionObject.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (ParticleSystemRenderer psr in psRenderers)
        {
            psr.sortingOrder = explosionSortingOrder;
        }

        SpriteRenderer[] spriteRenderers = explosionObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sortingOrder = explosionSortingOrder;
        }

        explosionObject.transform.SetAsLastSibling();

        Debug.Log($"Explosión forzada a ser visible - Layer: {explosionLayer}, Order: {explosionSortingOrder}");
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private void HideShip()
    {
        if (shipVisual != null)
        {
            shipVisual.SetActive(false);
            Debug.Log("shipVisual desactivado: " + shipVisual.name);
        }
        else
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in renderers)
            {
                if (explosionChild != null && r.transform.IsChildOf(explosionChild.transform))
                    continue;
                r.enabled = false;
            }

            Collider[] colliders = GetComponentsInChildren<Collider>(true);
            foreach (Collider c in colliders)
            {
                if (explosionChild != null && c.transform.IsChildOf(explosionChild.transform))
                    continue;
                c.enabled = false;
            }

            MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>(true);
            foreach (MonoBehaviour m in scripts)
            {
                if (m == this) continue;
                if (m == audioSource) continue;
                if (explosionChild != null && m.transform.IsChildOf(explosionChild.transform)) continue;
                m.enabled = false;
            }
        }
    }

    private void LoadDeathScene()
    {
        Debug.Log("Cargando escena de muerte...");
        if (!string.IsNullOrEmpty(deathSceneName))
        {
            SceneManager.LoadScene(deathSceneName);
        }
        else if (deathSceneIndex >= 0)
        {
            SceneManager.LoadScene(deathSceneIndex);
        }
        else
        {
            Debug.LogError("No se ha configurado una escena de muerte");
        }
    }

    public float GetNormalizedHealth() => (float)currentHealth / maxHealth;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsDead() => isDead;
}