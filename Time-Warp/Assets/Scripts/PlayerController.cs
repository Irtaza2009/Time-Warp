using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 12f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.05f;

    Rigidbody2D rb;
    Animator animator;
    Collider2D col;

    float horizontalInput;
    bool isGrounded;
    bool isJumping = false;  
    string currentAnim = "";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.BoxCast(
            col.bounds.center,
            col.bounds.size,
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        // Jump
        if (isGrounded && !isJumping && Input.GetButtonDown("Jump"))
        {
            Jump();
            PlayAnim("Player_Jump");
            isJumping = true;
        }

        // Flip sprite
        if (horizontalInput > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);

        HandleAnimations();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }


    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Animations
    void HandleAnimations()
    {
        if (isJumping)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

            // Wait until full animation finishes
            if (info.IsName("Player_Jump") && info.normalizedTime >= 1f)
            {
                isJumping = false;
            }
            else
            {
                return; 
            }
        }

        if (Mathf.Abs(horizontalInput) > 0.1f)
            PlayAnim("Player_Walk");
        else
            PlayAnim("Player_Idle");
    }

    void PlayAnim(string animName)
    {
        if (currentAnim == animName) return;

        animator.Play(animName);
        currentAnim = animName;
    }
}
