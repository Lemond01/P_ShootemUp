using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform target;      // arrastra aquí el Player
    public float height = 20f;    // Y de la cámara (fija)
    public float smoothTime = 0.12f;

    // Limites del nivel en X,Z (ajusta según tu nivel)
    public Vector2 levelMin = new Vector2(-50f, -50f); // xMin, zMin
    public Vector2 levelMax = new Vector2(50f, 200f);  // xMax, zMax

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = GetComponent<Camera>();
        // aseguramos ortográfica para topdown
        cam.orthographic = true;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Deseada: misma X/Z que target, Y fija
        Vector3 desired = new Vector3(target.position.x, height, target.position.z);

        // Calculamos los límites visibles de la cámara (extensiones)
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        // Clamp para que cámara no salga del nivel (así el player no queda fuera)
        float clampedX = Mathf.Clamp(desired.x, levelMin.x + horzExtent, levelMax.x - horzExtent);
        float clampedZ = Mathf.Clamp(desired.z, levelMin.y + vertExtent, levelMax.y - vertExtent);

        Vector3 clamped = new Vector3(clampedX, desired.y, clampedZ);

        // Suavizamos el movimiento
        transform.position = Vector3.SmoothDamp(transform.position, clamped, ref velocity, smoothTime);
    }
}


