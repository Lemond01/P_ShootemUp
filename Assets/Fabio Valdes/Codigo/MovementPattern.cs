using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementPattern { ZigZag, Circle, Straight, Sine }
    public MovementPattern pattern = MovementPattern.Straight;

    public float speed = 5f;
    public float amplitude = 3f;
    public float frequency = 2f;

    // ?? Límites del área
    public Vector2 xBounds = new Vector2(-10f, 10f);
    public Vector2 zBounds = new Vector2(-5f, 15f);

    private Vector3 startPosition;
    private float time;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime * speed;

        Vector3 newPosition = startPosition;

        switch (pattern)
        {
            case MovementPattern.Straight:
                newPosition += Vector3.back * time;
                break;

            case MovementPattern.ZigZag:
                newPosition += Vector3.back * time;
                newPosition.x += Mathf.PingPong(time * frequency, amplitude) - (amplitude / 2);
                break;

            case MovementPattern.Circle:
                newPosition.x += Mathf.Cos(time * frequency) * amplitude;
                newPosition.z -= time;
                newPosition.y += Mathf.Sin(time * frequency) * amplitude * 0.2f;
                break;

            case MovementPattern.Sine:
                newPosition += Vector3.back * time;
                newPosition.x += Mathf.Sin(time * frequency) * amplitude;
                break;
        }

        // ?? Aplicar límites
        newPosition.x = Mathf.Clamp(newPosition.x, xBounds.x, xBounds.y);
        newPosition.z = Mathf.Clamp(newPosition.z, zBounds.x, zBounds.y);

        transform.position = newPosition;
    }
}


