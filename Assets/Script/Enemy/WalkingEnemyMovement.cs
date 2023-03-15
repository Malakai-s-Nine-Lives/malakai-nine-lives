using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyMovement : MonoBehaviour
{
    public Transform player;  // To react to player movement
    public int damage = 25;  // Damage value for enemy
    public float moveSpeed = 0.5f;  // Speed of enemy

    // Get the movement for the enemy
    private Rigidbody2D enemy_body;
    private Vector2 movement;

    // Booleans for the flipping direction facing
    private bool facingLeft;
    private SpriteRenderer sprite_render;

    // For setting health
    public int maxHealth = 100;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        enemy_body = this.GetComponent<Rigidbody2D>();

        // Freeze rotation and movement in y-axis
        enemy_body.freezeRotation = true;
        enemy_body.angularVelocity = 0f;
        enemy_body.constraints = RigidbodyConstraints2D.FreezePositionY;
        sprite_render = this.GetComponent<SpriteRenderer>();

        // Set health as max
        currentHealth = maxHealth;

        // Get position of player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // Initialize the check which way the enemy is facing
        Flip(direction);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate by caluclated angle to face player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
        Flip(movement);  // Flip the image to match the direction to face
        transform.rotation = Quaternion.Euler(0, 0, 0);  // Ensure no rotation
    } 

    private void FixedUpdate()
    {
        MoveEnemy(movement);  // Have enemy follow player
    }

    // Move enemy position based on where the player is
    private void MoveEnemy(Vector2 direction)
    {
        enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void Flip(Vector2 direction)
    {
        if (direction[0] < 0 && !facingLeft)
        {
            sprite_render.flipX = true;
            facingLeft = true;
        } else if (direction[0] >= 0 && facingLeft)
        {
            sprite_render.flipX = false;
            facingLeft = false;
        }
        Debug.Log(facingLeft);
    }

    // Accessed by enemy attack scripts to give damage to player
    public void TakeDamage()
    {
        currentHealth -= damage;
    }
}
