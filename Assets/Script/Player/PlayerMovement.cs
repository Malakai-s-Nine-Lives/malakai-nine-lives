using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // For setting health and progress
    public HealthBar healthBar;
    public ProgressBar progressBar;
    public int maxPoints = 100;
    public int currentPoints = 0;
    public int maxHealth = 100;
    private int currentHealth;

    // For player attacks
    public float attackPointValue = 3f;

    // For controlling player movement
    public float speed = 8f;
    public float jumpingPower = 3f;
    public float jumpCooldown = 1f;
    public LayerMask groundLayer;
    private float horizontal;
    private bool isFacingRight = true;
    private float jumpTime;

    // Additional Unity components
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        // Grab references for rigidbody and animator from object
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // Initialize health and progress bars
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        progressBar.SetMaxPoints(maxPoints);

    }

    void Update()
    {
        // Get player input for horizontal direction
        horizontal = Input.GetAxisRaw("Horizontal");

        // Set animator params
        anim.SetBool("walk", horizontal != 0);
        anim.SetBool("grounded", IsGrounded());

        // Jump logic
        if (jumpTime > jumpCooldown && Input.GetKey(KeyCode.Space) && IsGrounded())
        {
                print("JUMPING");
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                anim.SetTrigger("jump");
                jumpTime = 0;
        } else
        {
            jumpTime += Time.deltaTime;
        }

        // Update the health bar and progress bar
        healthBar.SetHealth(currentHealth);
        progressBar.SetPoints(currentPoints);

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

    // Accessed by enemy attack scripts to give damage to player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    // Accessed by enemy scripts when they die to award their point amount to the player
    public void TakePoints(int points)
    {
        currentPoints += points;
    }
}