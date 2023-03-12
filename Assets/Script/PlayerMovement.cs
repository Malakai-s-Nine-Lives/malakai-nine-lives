using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // for our game logic
    [SerializeField] private float speed = 8f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float attackPointValue = 3f;
    [SerializeField] private float jumpingPower = 3f;
    [SerializeField] private LayerMask groundLayer;

    // for the game engine
    private float horizontal;
    private bool isFacingRight = true;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private float jumpCooldown;


    private void Awake()
    {
        // Grab references for rigidbody and animator from object
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Set animator params
        anim.SetBool("walk", horizontal != 0);
        anim.SetBool("grounded", IsGrounded());

        // Jump logic
        if (jumpCooldown > 1f)
        {
            if (Input.GetKey(KeyCode.Space) && IsGrounded())
            {
                print("JUMPING");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                anim.SetTrigger("jump");
                jumpCooldown = 0;
            }
        } else
        {
            jumpCooldown += Time.deltaTime;
        }


        /*
        if (rb.velocity.y > 0f)
        {
            print("FALLING");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            // anim.SetTrigger("fall")
        }
        */
        // Flip the player when moving left-right
        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    // use 2D raycasting to determine if player is on the ground
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}