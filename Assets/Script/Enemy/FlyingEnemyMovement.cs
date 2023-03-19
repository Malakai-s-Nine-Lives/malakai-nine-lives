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

    // These are used for pathfinding
    Vector2[] path;
    private int targetIndex;

    private Vector2 playerPos;  // Store Player position

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
        playerPos = player.position;

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

        Flip(direction); // Get correct position initially

        // Initially request A* to attack teh player
        PathRequest.RequestPath(transform.position, player.position, OnPathFound);

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
        if (playerPos != (Vector2) player.position)  // If player has moved
        {
            // Get an optimal path of attack
            PathRequest.RequestPath(transform.position, player.position, OnPathFound);
            playerPos = player.position;  // Update player's current position
        }
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)  // Successful path found
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");  // Stop moving if we were before
            StartCoroutine("FollowPath");  // Attack with newly found path
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)  // Only move if we have somewhere to move
        {
            Vector2 currentWaypoint = path[0];  // Get the waypoint to move to
            while (true)
            {
                // Make sure we have made the previous move
                if ((Vector2)transform.position == currentWaypoint)
                {
                    targetIndex++;  // Where we plan to go
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                enemy_body.rotation = rotation_angle;  // Rotate enemy based on direction of travel

                // Move
                transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
                yield return null;

            }
        }
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
