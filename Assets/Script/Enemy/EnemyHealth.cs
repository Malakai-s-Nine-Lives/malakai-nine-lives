using UnityEngine;

/*
 * This module satisfies:
 *   - Functional requirement 6.1
 *   - Functional requirement 8.1
 */
public class EnemyHealth : MonoBehaviour
{
    // For setting health and giving progress points to player
    public int maxHealth = 100;
    public int pointValue = 100;
    public float maxCorpseTime = 5f;
    public PlayerProgress player;
    private int currentHealth;
    private float corpseTimer = 0;
    public LayerMask groundLayer;
    private Collider2D boxCollider;

    // Additional Unity components
    private Animator anim;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Grab reference to animator
        anim = GetComponent<Animator>();

        // Grab reference to rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Only deactivate the gameobject once it has hit the ground
        boxCollider = GetComponent<Collider2D>();

        // Intialize health
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // If the enemy is dead, check to see if the corpse needs to be removed yet
        if (currentHealth <= 0)
        {
            corpseTimer += Time.deltaTime;

            // Get rid of enemy body on screen if corpse timer is up
            if (corpseTimer > maxCorpseTime)
            {
                gameObject.SetActive(false);
            }

            // check to see if body has hit the ground yet, if so freeze the body
            // if the player is touching the ground then disable its collider and freeze it
            if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer).collider != null)
            {
                // Make sure it stays in the same position it died in
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                // Deactivate the enemy
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    // Accessed by player attack script to deal damage to enemy
    public void TakeDamage(int damage)
    {
        // Play hurt animation and decrement health
        anim.SetTrigger("hurt");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // Kill the enemy and give point value to the player
            anim.SetBool("die", true);
            GetComponent<EnemyAttack>().enabled = false;

            // Give player the game points
            player.TakePoints(pointValue);

            if (GetComponent<WalkingEnemyMovement>())
            {
                GetComponent<WalkingEnemyMovement>().enabled = false;
            }
            else
            {
                // Set gravity so crow can fall to the ground
                rb.gravityScale = 2.0f;
                GetComponent<FlyingEnemyMovement>().enabled = false;
            }
        }
    }
}