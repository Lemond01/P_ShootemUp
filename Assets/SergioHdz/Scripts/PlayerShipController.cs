using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShipController : MonoBehaviour
{
    [Header("Aceleraciones (ajusta para el 'feeling')")]
    public float baseForwardAcceleration = 20f;   // aceleración que siempre empuja hacia delante
    public float forwardBoost = 30f;              // extra cuando mantienes W
    public float backwardReduction = 10f;         // cuando presionas S reduce la aceleración (usa valor positivo)
    public float minForwardAcceleration = 2f;     // no dejar que quede 0 (si quieres que no vaya hacia atrás)

    [Header("Strafing y límites")]
    public float strafeAcceleration = 25f;        // aceleración lateral (A/D)
    public float maxForwardSpeed = 40f;
    public float maxStrafeSpeed = 12f;

    [Header("Drag y suavizado")]
    public float linearDrag = 1.5f;               // arrastre general (o usa rb.drag en inspector)
    public float velocityClampSmooth = 8f;        // suavizado cuando limitamos velocidad

    [Header("Visual: rotación del modelo (hijo) para tilt)")]
    public Transform modelTransform;              // asigna aquí el child 'Model'
    public float tiltAngle = 18f;                 // inclinación máxima en roll
    public float tiltSpeed = 6f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Evitar movimiento en Y y rotaciones físicas que "tumbling"
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = linearDrag;
    }

    void FixedUpdate()
    {
        // Input clásico (WASD / arrow keys)
        float h = Input.GetAxis("Horizontal"); // A/D -> -1..1
        float v = Input.GetAxis("Vertical");   // S = -1, W = +1

        // --- cálculo de aceleración hacia delante ---
        float forwardAcc = baseForwardAcceleration;
        if (v > 0f)
            forwardAcc += v * forwardBoost;               // W aumenta la aceleración
        else if (v < 0f)
            forwardAcc += v * backwardReduction;          // S (v negativo) reduce la aceleración un poco

        // Evita que la aceleración hacia delante sea negativa (si no quieres inversión)
        forwardAcc = Mathf.Max(forwardAcc, minForwardAcceleration);

        Vector3 forceForward = transform.forward * forwardAcc;
        Vector3 forceStrafe  = transform.right * h * strafeAcceleration;

        // Aplica fuerza como aceleración (es independiente de la masa)
        rb.AddForce((forceForward + forceStrafe) * Time.fixedDeltaTime, ForceMode.Acceleration);

        // --- clamp de velocidades por componentes (para mantener control) ---
        Vector3 vel = rb.linearVelocity;
        vel.y = 0f; // seguro extra en caso de que algo afecte Y

        // extraer componentes en local (forward / right)
        float forwardVel = Vector3.Dot(vel, transform.forward);
        float strafeVel  = Vector3.Dot(vel, transform.right);

        forwardVel = Mathf.Clamp(forwardVel, -maxForwardSpeed, maxForwardSpeed);
        strafeVel  = Mathf.Clamp(strafeVel, -maxStrafeSpeed, maxStrafeSpeed);

        // reconstruimos la velocidad con suavizado para evitar saltos bruscos
        Vector3 targetVel = transform.forward * forwardVel + transform.right * strafeVel;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVel, Time.fixedDeltaTime * velocityClampSmooth);
    }

    void Update()
    {
        // Rotación visual (tilt) aplicada al child model para feedback visual
        if (modelTransform != null)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float targetRoll = -h * tiltAngle;          // roll con movimiento lateral
            float targetPitch = -v * (tiltAngle * 0.4f);// pitch sutil con aceleración

            Quaternion targetLocal = Quaternion.Euler(targetPitch, 0f, targetRoll);
            modelTransform.localRotation = Quaternion.Slerp(modelTransform.localRotation, targetLocal, Time.deltaTime * tiltSpeed);
        }
    }
}

