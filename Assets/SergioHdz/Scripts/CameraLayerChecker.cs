using UnityEngine;

public class CameraLayerChecker : MonoBehaviour
{
    void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Debug.Log($"Cámara: {mainCamera.name}");
            Debug.Log($"Culling Mask: {mainCamera.cullingMask}");
            Debug.Log($"Clear Flags: {mainCamera.clearFlags}");
            Debug.Log($"Depth: {mainCamera.depth}");
        }
        
        // Verificar todas las cámaras en escena
        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in allCameras)
        {
            Debug.Log($"Cámara encontrada: {cam.name} - Depth: {cam.depth} - Culling: {cam.cullingMask}");
        }
    }
}
