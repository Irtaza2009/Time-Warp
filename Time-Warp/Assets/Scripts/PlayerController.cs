using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float acceleration = 15f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 12f;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    Collider2D col;
    float horizontalInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (col.IsTouchingLayers(groundLayer) && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - rb.linearVelocity.x;
        float movement = Mathf.Clamp(speedDifference, -acceleration * Time.fixedDeltaTime, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);
    }

    void Jump()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }
}
