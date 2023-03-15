using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    public Transform player;  // To react to player movement
    public int damage = 50;  // Damage value for enemy
    public float moveSpeed = 0.5f;  // Speed of enemy

    // Get the movement for the enemy
    private Rigidbody2D enemy_body;
    private Vector2 movement;
    private float rotation_angle;

    // Booleans for the rotation direction of the flying enemy
    private bool facingLeft;
    private bool facingdown;
    private SpriteRenderer sprite_render;

    // For setting health
    public int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        enemy_body = this.GetComponent<Rigidbody2D>();
        sprite_render = this.GetComponent<SpriteRenderer>();

        // Set health as max
        currentHealth = maxHealth;

        // Get position of player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // Initialize the left and down values depending on current enemy
        // rotation
        if (direction[0] < 0)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
        if (direction[1] < 0)
        {
            facingdown = true;
        } else
        {
            facingdown = false;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate by caluclated angle to face player
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        movement = direction;
        rotation_angle = angle;
        Flip(movement);  // Flip the image to match the direction rotated

    }

    private void FixedUpdate()
    {
        MoveEnemy(movement);  // Have enemy follow player
    }

    // Move enemy position based on where the player is
    private void MoveEnemy(Vector2 direction)
    {
        enemy_body.rotation = rotation_angle;
        enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    // Flip the direction based on the quadrant the rotation is
    private void Flip(Vector2 direction)
    {
        if (IsDown(movement) && IsLeft(movement))  // Quadrant 4
        {
            // Make sure we havent flipped in this direction already
            if (IsDown(movement) != facingdown || IsLeft(movement) != facingLeft)
            {
                // Flip and save new direction
                sprite_render.flipY = true;
                sprite_render.flipX = false;
                facingdown = true;
                facingLeft = true;
            }
        }
        else if (!IsDown(movement) && !IsLeft(movement))  // Quadrant 2
        {
            if (IsDown(movement) != facingdown || IsLeft(movement) != facingLeft)
            {
                sprite_render.flipY = false;
                sprite_render.flipX = false;
                facingdown = false;
                facingLeft = false;
            }
        }
        else if (!IsDown(movement) && IsLeft(movement))  // Quadrant 3
        {
            if (IsDown(movement) != facingdown || IsLeft(movement) != facingLeft)
            {
                sprite_render.flipY = true;
                sprite_render.flipX = false;
                facingdown = false;
                facingLeft = true;
            }
        }
        else if (IsDown(movement) && !IsLeft(movement))  // Quadrant 1
        {
            if (IsDown(movement) != facingdown || IsLeft(movement) != facingLeft)
            {
                sprite_render.flipY = false;
                sprite_render.flipX = false;
                facingdown = true;
                facingLeft = false;
            }
        }
    }

    // Check the x value of the coordinate to determine if left or not
    private bool IsLeft(Vector2 direction)
    {
        if (direction[0] < 0)
        {
            return true;
        } else
        {
            return false;
        }

    }

    // Check the y value of the coordinate to determine if left or not
    private bool IsDown(Vector2 direction)
    {
        if (direction[1] < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Accessed by enemy attack scripts to give damage to player
    public void TakeDamage()
    {
        currentHealth -= damage;
    }

}
