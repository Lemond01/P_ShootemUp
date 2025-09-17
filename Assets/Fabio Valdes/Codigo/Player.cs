using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player: MonoBehaviour
{
    public float moveSpeed = 10f;
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float xLimit = 7.5f;
    public float zLimit = 4f;

    private CharacterController cc;
    private Vector3 moveInput;
    private bool isDashing = false;
    private float dashTime;
    private float lastDash;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Input
        moveInput = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveInput.z = 1f;
        if (Input.GetKey(KeyCode.S)) moveInput.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveInput.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveInput.x = 1f;
        moveInput = moveInput.normalized;

        // Dash
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDash + dashCooldown && moveInput.magnitude > 0)
        {
            isDashing = true;
            dashTime = Time.time;
            lastDash = Time.time;
        }

        // Movimiento
        Vector3 velocity = moveInput * (isDashing ? dashSpeed : moveSpeed);
        cc.Move(velocity * Time.deltaTime);

        // Limites
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        pos.z = Mathf.Clamp(pos.z, -zLimit, zLimit);
        transform.position = pos;

        if (isDashing && Time.time >= dashTime + dashDuration)
        {
            isDashing = false;
        }
    }
}



