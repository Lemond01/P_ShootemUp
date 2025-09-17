using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // player
    public float followDistance = 12f;  // distancia atrás
    public float height = 6f;           // altura
    public float smoothTime = 0.12f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 lookOffset = new Vector3(0, 1.5f, 0); // donde mira la cámara respecto al target

    void LateUpdate()
    {
        if (target == null) return;

        // posición deseada: detrás del target en su forward, más algo de altura
        Vector3 desiredPos = target.position - target.forward * followDistance + Vector3.up * height;

        // suavizado
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);

        // mirar al objetivo ligeramente por encima del centro
        Vector3 lookPoint = target.position + lookOffset;
        transform.LookAt(lookPoint);
    }
}

