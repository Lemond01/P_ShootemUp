using UnityEngine;

public class ParallaxRepeater : MonoBehaviour
{
    public Camera cam;                  // cámara ortográfica
    public Transform planeA;
    public Transform planeB;
    public float planeLength = 60f;     // tamaño efectivo en Z de cada plano (ajusta con el scale)
    public float parallaxFactor = 0.6f; // 0 = no se mueve, 1 = se mueve con la cámara
    private Vector3 lastCamPos;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        lastCamPos = cam.transform.position;
    }

    void Update()
    {
        Vector3 camPos = cam.transform.position;
        Vector3 delta = camPos - lastCamPos;

        // Parallax: movemos ligeramente los planos según el movimiento de la cámara
        Vector3 parallaxMove = new Vector3(delta.x * parallaxFactor, 0f, delta.z * parallaxFactor);
        planeA.position += parallaxMove;
        planeB.position += parallaxMove;

        // Recycle en Z (para avance en Z+)
        if (camPos.z - planeA.position.z > planeLength)
        {
            // A quedó demasiado atrás -> lo colocamos delante de B
            planeA.position = new Vector3(planeA.position.x, planeA.position.y, planeB.position.z + planeLength);
        }
        else if (camPos.z - planeB.position.z > planeLength)
        {
            planeB.position = new Vector3(planeB.position.x, planeB.position.y, planeA.position.z + planeLength);
        }

        // Recycle cuando vas hacia atrás (Z-)
        if (planeA.position.z - camPos.z > planeLength)
        {
            planeA.position = new Vector3(planeA.position.x, planeA.position.y, planeB.position.z - planeLength);
        }
        else if (planeB.position.z - camPos.z > planeLength)
        {
            planeB.position = new Vector3(planeB.position.x, planeB.position.y, planeA.position.z - planeLength);
        }

        lastCamPos = camPos;
    }
}