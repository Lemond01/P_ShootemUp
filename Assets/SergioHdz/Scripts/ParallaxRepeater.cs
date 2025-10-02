using UnityEngine;

public class ParallaxRepeater : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Sprite sprite;   // Imagen que arrastras
    [SerializeField] private float speed = 2f; // Velocidad del parallax
    [SerializeField] private Camera mainCamera;

    private GameObject image1;
    private GameObject image2;
    private float spriteWidth;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Crear dos imágenes con el sprite
        image1 = CreateImage("Image1", Vector3.zero);
        image2 = CreateImage("Image2", new Vector3(sprite.bounds.size.x, 0, 0));

        spriteWidth = sprite.bounds.size.x;
    }

    private void Update()
    {
        // Mover en la dirección local del padre (izquierda local)
        Vector3 localLeft = transform.TransformDirection(Vector3.left);

        image1.transform.position += localLeft * speed * Time.deltaTime;
        image2.transform.position += localLeft * speed * Time.deltaTime;

        // Revisar visibilidad y reposicionar
        CheckAndReposition(image1, image2);
        CheckAndReposition(image2, image1);
    }

    private GameObject CreateImage(string name, Vector3 localPosition)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = transform;
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        return go;
    }

    private void CheckAndReposition(GameObject current, GameObject other)
    {
        // Revisar si el centro de la imagen ya salió por completo de la cámara
        Vector3 viewPos = mainCamera.WorldToViewportPoint(current.transform.position);

        if (viewPos.x < -1f) // salió por la izquierda
        {
            // Reposicionar usando el eje local del padre
            Vector3 offset = transform.TransformDirection(Vector3.right) * spriteWidth;

            current.transform.position = other.transform.position + offset;
        }
    }
}