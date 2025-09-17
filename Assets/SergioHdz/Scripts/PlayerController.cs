using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Cruise (velocidad constante hacia delante)")]
    public float cruiseTargetSpeed = 6f;        // velocidad objetivo del cruise (m/s)
    public float minCruiseSpeed = 2f;           // menor velocidad que puede alcanzar con S
    public float cruiseRecoverRate = 1.5f;      // cuanto recupera por segundo hacia cruiseTarget
    public float cruiseReduceRate = 3f;         // cuanto reduce la cruise por segundo cuando presionas S

    [Header("Control del jugador (WASD)")]
    public float strafeSpeed = 5f;              // velocidad lateral (A/D)
    public float localInputForwardSpeed = 3f;   // velocidad que aporta W (no sube el cruise)
    public float velocityLerp = 10f;            // suavizado de la velocidad

    Rigidbody rb;
    float currentCruiseSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Bloqueamos la posición Y para que la nave nunca suba/baje
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        currentCruiseSpeed = cruiseTargetSpeed;
    }

    void FixedUpdate()
    {
        // Obtener input (mapas por defecto de Unity: Horizontal = A/D, Vertical = W/S)
        float h = Input.GetAxisRaw("Horizontal"); // -1..1 (A/D)
        float v = Input.GetAxisRaw("Vertical");   // -1..1 (S/W) -> S es negativo

        // Si presionas S (v < 0) reducimos la velocidad de cruise un poco
        if (v < -0.01f)
        {
            currentCruiseSpeed = Mathf.Max(minCruiseSpeed, currentCruiseSpeed + v * cruiseReduceRate * Time.fixedDeltaTime);
            // Nota: v es negativo, así que se resta a currentCruiseSpeed
        }
        else
        {
            // Si no presionas S, la cruise se recupera suavemente hacia su target
            currentCruiseSpeed = Mathf.MoveTowards(currentCruiseSpeed, cruiseTargetSpeed, cruiseRecoverRate * Time.fixedDeltaTime);
        }

        // Auto-advance: vector que representa la "velocidad de crucero" hacia adelante (mundo Z +)
        // Usamos Vector3.forward para que la cruise no dependa de la rotación de la nave.
        Vector3 autoForward = Vector3.forward * currentCruiseSpeed;

        // Movimiento por input del jugador (W/A/S/D)
        // W añadirá un empuje "localInputForwardSpeed" (no modifica currentCruiseSpeed)
        Vector3 inputMove = new Vector3(h * strafeSpeed, 0f, v * localInputForwardSpeed);

        // Velocidad deseada = cruise + input
        Vector3 desiredVelocity = autoForward + inputMove;
        desiredVelocity.y = 0f; // por seguridad

        // Aplicamos suavizado para evitar cambios bruscos
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, velocityLerp * Time.fixedDeltaTime);
    }
}


