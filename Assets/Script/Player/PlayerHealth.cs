using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // For setting health
    public HealthBar healthBar;
    public int maxHealth = 100;
    private int currentHealth;

    // Additional Unity components
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize health
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;

        // Get animator component
        anim = GetComponent<Animator>();
    }

    // Accessed by enemy attack scripts to give damage to the player
    public void TakeDamage(int damage)
    {
        // Play hurt animation and decrement current health
        anim.SetTrigger("hurt");
        currentHealth -= damage;

        // Update health bar
        healthBar.SetHealth(currentHealth);
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            // Player dies!
            anim.SetBool("die", true);

            // Make sure body has hit the floor
            if (GetComponent<PlayerMovement>().IsGrounded())
            {
                // Deactivate the player (and eventually move to death cinematic once we have that)
                GetComponent<Collider2D>().enabled = false;
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerAttack>().enabled = false;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
}
