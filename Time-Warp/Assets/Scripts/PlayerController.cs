using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float acceleration = 15f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 12f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.05f;

    Rigidbody2D rb;
    Animator animator;
    Collider2D col;

    float horizontalInput;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Read input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Ground Check
        isGrounded = Physics2D.BoxCast(
            col.bounds.center,
            col.bounds.size,
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // --- Animation ---
        animator.SetBool("IsWalking", Mathf.Abs(horizontalInput) > 0.1f);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);

        // --- Flip Sprite ---
        if (horizontalInput > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        // Smooth horizontal acceleration
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - rb.linearVelocity.x;

        float movement = Mathf.Clamp(
            speedDifference,
            -acceleration * Time.fixedDeltaTime,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
