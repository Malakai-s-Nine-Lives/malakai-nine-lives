using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // For setting health
    public HealthBar healthBar;
    public int maxHealth = 100; 
    private int currentHealth;
    public float blockTime = 1f;
    private float damageTimer = 0.00f;
    private float deathTimer = 0.00f;
    private float corpseTime = 2.00f;
    public bool blocking = false;
    public GameObject DeathMenuUI;

    // Additional Unity components
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize health
        Debug.Log("mode is" + PlayerPrefs.GetString("mode"));
        Debug.Log("current health is: " + PlayerPrefs.GetInt("health") + "out of " + maxHealth);
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = PlayerPrefs.GetString("mode") == "hard" ? PlayerPrefs.GetInt("health") : maxHealth;
        healthBar.SetHealth(currentHealth);

        // Get animator component
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        // set blocking variable if player is currently blocking
        blocking = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        if (blocking) anim.Play("Fright");

        // if the death timer has started, continue it
        if (currentHealth <= 0) deathTimer += Time.deltaTime;

        // call TakeDamage to kill
        if (deathTimer > corpseTime) PlayerDies();
    }

    // Accessed by enemy attack scripts to give damage to the player
    public void TakeDamage(int damage)
    {
        damageTimer = 0;
        // Give the player a set amount of time to avoid taking damage by blocking
        while (damageTimer < blockTime)
        {
            // If the player is taking damage, give player some time to block
            damageTimer += Time.deltaTime;

            // check to see if user has pressed block button
            if (blocking)
            {
                Debug.Log("down arrow pressed, block");
                // block attack
                damageTimer = 0;
                return;
            }
        }

        // Play hurt animation and decrement current health
        anim.SetTrigger("hurt");
        currentHealth -= damage;

        // Update health bar
        healthBar.SetHealth(currentHealth);
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            PlayerDies();
        }
    }

    public void PlayerDies()
    {
        // Player dies!
        anim.SetBool("die", true);

        // Make sure body has hit the floor
        if (GetComponent<PlayerMovement>().IsGrounded() ||  deathTimer > corpseTime)
        {
            // Deactivate the player
            GetComponent<Collider2D>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke("toDeathMenu", 2);  // This will take us to the death menu in 2 seconds
        }
    }

    // For hard mode, saves the current health of the player
    public void SaveHealth()
    {
        PlayerPrefs.SetInt("health", currentHealth);
    }

    void toDeathMenu()
    {
        DeathMenuUI.SetActive(true);  // Open Death Menu
    }
}
