using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float height = 20f;
    public float smoothTime = 0.12f;

    [Header("Configuración de Capas")]
    public LayerMask cullingMask = -1; // Everything por defecto

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        
        // Asegurar que la cámara renderice todas las capas necesarias
        cam.cullingMask = cullingMask;
        
        Debug.Log($"CameraFollow iniciado - Culling Mask: {cam.cullingMask}");
    }

    void LateUpdate()
    {
        if (target == null) 
        {
            // Buscar automáticamente al jugador si no está asignado
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log($"CameraFollow: Target asignado automáticamente - {target.name}");
            }
            else
            {
                return;
            }
        }

        Vector3 desired = new Vector3(target.position.x, height, target.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }

    // Método para forzar que la cámara renderice una capa específica
    public void AddLayerToCullingMask(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer != -1)
        {
            cam.cullingMask |= (1 << layer);
            Debug.Log($"Capa {layerName} añadida al culling mask");
        }
    }

    // Método para forzar que la cámara renderice una capa específica
    public void AddLayerToCullingMask(int layer)
    {
        if (layer >= 0 && layer < 32)
        {
            cam.cullingMask |= (1 << layer);
            Debug.Log($"Capa {layer} añadida al culling mask");
        }
    }
}


