using UnityEngine;

public class ParallaxRepeater : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Sprite sprite;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Camera mainCamera;

    [Header("Escala de Imágenes")]
    [SerializeField] private Vector3 imageScale = Vector3.one;
    [SerializeField] private bool uniformScaling = true;

    [Header("Dirección del Movimiento")]
    [SerializeField] private bool moveVertical = true;

    [Header("Configuración de Muerte")]
    [SerializeField] private float slowDownSpeed = 0.5f; // Velocidad cuando la nave muere
    [SerializeField] private float slowDownDuration = 2f; // Duración del slowdown
    [SerializeField] private HealthSystem playerHealthSystem;

    private GameObject image1;
    private GameObject image2;
    private float spriteWidth;
    private float spriteHeight;
    private float originalSpeed;
    private bool isSlowingDown = false;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Calcular dimensiones del sprite considerando la escala
        float baseWidth = sprite.bounds.size.x;
        float baseHeight = sprite.bounds.size.y;
        spriteWidth = baseWidth * imageScale.x;
        spriteHeight = baseHeight * imageScale.y;

        // Guardar velocidad original
        originalSpeed = speed;

        // Crear dos imágenes con el sprite
        Vector3 initialPosition = Vector3.zero;
        Vector3 secondImagePosition = moveVertical ? 
            new Vector3(0, spriteHeight, 0) : 
            new Vector3(spriteWidth, 0, 0);

        image1 = CreateImage("Image1", initialPosition);
        image2 = CreateImage("Image2", secondImagePosition);

        // Buscar el HealthSystem del jugador si no está asignado
        if (playerHealthSystem == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealthSystem = player.GetComponent<HealthSystem>();
            }
        }

        // Suscribirse al evento de muerte
        if (playerHealthSystem != null)
        {
            playerHealthSystem.OnShipHidden += OnShipHidden;
            Debug.Log("ParallaxRepeater suscrito al evento de muerte del jugador");
        }
        else
        {
            Debug.LogWarning("No se encontró HealthSystem del jugador para el ParallaxRepeater");
        }
    }

    private void Update()
    {
        // Determinar dirección del movimiento
        Vector3 movementDirection = moveVertical ? 
            transform.TransformDirection(Vector3.down) : 
            transform.TransformDirection(Vector3.left);

        image1.transform.position += movementDirection * speed * Time.deltaTime;
        image2.transform.position += movementDirection * speed * Time.deltaTime;

        // Revisar visibilidad y reposicionar
        CheckAndReposition(image1, image2);
        CheckAndReposition(image2, image1);
    }

    private void OnShipHidden()
    {
        if (!isSlowingDown)
        {
            StartCoroutine(SlowDownParallax());
        }
    }

    private System.Collections.IEnumerator SlowDownParallax()
    {
        isSlowingDown = true;
        Debug.Log("Iniciando slowdown del parallax");

        float startSpeed = speed;
        float endSpeed = slowDownSpeed;
        float elapsedTime = 0f;

        // Transición suave a la velocidad reducida
        while (elapsedTime < slowDownDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slowDownDuration;
            speed = Mathf.Lerp(startSpeed, endSpeed, t);
            yield return null;
        }

        speed = endSpeed;
        Debug.Log($"Parallax reducido a velocidad: {speed}");
    }

    private GameObject CreateImage(string name, Vector3 localPosition)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = transform;
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.identity;
        
        if (uniformScaling)
        {
            float scale = imageScale.x;
            go.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            go.transform.localScale = imageScale;
        }

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        return go;
    }

    private void CheckAndReposition(GameObject current, GameObject other)
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(current.transform.position);

        if (moveVertical)
        {
            if (viewPos.y < -1f)
            {
                Vector3 offset = transform.TransformDirection(Vector3.up) * spriteHeight;
                current.transform.position = other.transform.position + offset;
            }
        }
        else
        {
            if (viewPos.x < -1f)
            {
                Vector3 offset = transform.TransformDirection(Vector3.right) * spriteWidth;
                current.transform.position = other.transform.position + offset;
            }
        }
    }

    // Método para reiniciar la velocidad (útil si hay respawn)
    public void ResetSpeed()
    {
        speed = originalSpeed;
        isSlowingDown = false;
        Debug.Log("Velocidad del parallax reiniciada");
    }

    // Método público para cambiar la velocidad manualmente
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        originalSpeed = newSpeed;
    }

    // Método para cambiar la dirección del movimiento
    public void SetMovementDirection(bool vertical)
    {
        if (moveVertical != vertical)
        {
            moveVertical = vertical;
            ResetImagePositions();
        }
    }

    private void UpdateImageScales()
    {
        if (image1 == null || image2 == null) return;

        float baseWidth = sprite.bounds.size.x;
        float baseHeight = sprite.bounds.size.y;
        spriteWidth = baseWidth * imageScale.x;
        spriteHeight = baseHeight * imageScale.y;

        if (uniformScaling)
        {
            float scale = imageScale.x;
            image1.transform.localScale = new Vector3(scale, scale, scale);
            image2.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            image1.transform.localScale = imageScale;
            image2.transform.localScale = imageScale;
        }

        ResetImagePositions();
    }

    private void ResetImagePositions()
    {
        if (image1 == null || image2 == null) return;

        image1.transform.localPosition = Vector3.zero;

        if (moveVertical)
        {
            image2.transform.localPosition = new Vector3(0, spriteHeight, 0);
        }
        else
        {
            image2.transform.localPosition = new Vector3(spriteWidth, 0, 0);
        }
    }

    public void SetImageScale(Vector3 newScale)
    {
        imageScale = newScale;
        UpdateImageScales();
    }

    public void SetImageScale(float uniformScale)
    {
        imageScale = new Vector3(uniformScale, uniformScale, uniformScale);
        UpdateImageScales();
    }

    public void SetUniformScaling(bool uniform)
    {
        uniformScaling = uniform;
        UpdateImageScales();
    }

    public Vector3 GetCurrentScale()
    {
        return imageScale;
    }

    public bool IsMovingVertical()
    {
        return moveVertical;
    }

    public float GetCurrentSpeed()
    {
        return speed;
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento para evitar memory leaks
        if (playerHealthSystem != null)
        {
            playerHealthSystem.OnShipHidden -= OnShipHidden;
        }
    }
}