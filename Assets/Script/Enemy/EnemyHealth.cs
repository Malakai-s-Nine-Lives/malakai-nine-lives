using UnityEngine;

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

    // Additional Unity components
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Grab reference to animator
        anim = GetComponent<Animator>();

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

            // Only deactivate the gameobject once it has hit the ground
            Collider2D boxCollider = GetComponent<Collider2D>();
            if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer).collider != null)
            {
                // Give player the game points
                player.TakePoints(pointValue);

                // Deactivate the enemy
                GetComponent<Collider2D>().enabled = false;
                if (GetComponent("WalkingEnemyMovement"))
                {
                    GetComponent<WalkingEnemyMovement>().enabled = false;
                } else
                {
                    GetComponent<FlyingEnemyMovement>().enabled = false;
                }
                // Make sure it stays in the same position it died in
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
}