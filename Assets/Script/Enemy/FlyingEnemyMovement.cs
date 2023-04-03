using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    public Transform player;  // To react to player movement
    public int damage = 50;  // Damage value for enemy
    public float moveSpeed = 0.5f;  // Speed of enemy
    public int waitSeconds = 0;  // Time to wait before searching

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
    public SpriteRenderer sprite_render;  // The enemy's sprite render

    private bool waitDone = false;

    // Additional Unity Components
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Wait for specified seconds before deploying
        StartCoroutine(wait());
        // Get enemy body
        enemy_body = this.GetComponent<Rigidbody2D>();
        sprite_render = this.GetComponent<SpriteRenderer>();

        // Get animator
        anim = GetComponent<Animator>();

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

    }

    IEnumerator wait()
    {
        sprite_render.enabled = false;
        yield return new WaitForSeconds(waitSeconds);  // Wait before moving
        waitDone = true;  // Update bool so update and fixed update can run
        sprite_render.enabled = true;
        // Initially request A* to attack the player
        PathRequest.RequestPath(transform.position, player.position, OnPathFound);
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitDone)
        {
            return;
        }
        // Rotate by caluclated angle to face player
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        movement = direction;
        rotation_angle = angle;
        Flip(movement);  // Flip the image to match the direction rotated

        // Set animation variables
        anim.SetBool("fall", enemy_body.velocity.y < 0);

    }

    private void FixedUpdate()
    {
        if (!waitDone)
        {
            return;
        }
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
            Debug.Log("path length" + path.Length);
            Vector2 currentWaypoint = path[0];  // Get the waypoint to move to
            while (true)
            {
                // Make sure we have made the previous move
                if ((Vector2)transform.position == currentWaypoint)
                {
                    targetIndex++;  // Where we plan to go
                    if (targetIndex >= path.Length || path.Length == 1)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                enemy_body.rotation = rotation_angle;  // Rotate enemy based on direction of travel
                // Move
                transform.position = (Vector2) Vector2.MoveTowards((Vector2) transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
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
                Vector3 localScale = transform.localScale;

                // Only flip in x if we were in the wrong direction to begin with
                if (localScale.x < 0)
                {
                    localScale.x *= -1f;
                }

                // Only flip in y if we were in the wrong direction to begin with
                if (localScale.y > 0)
                {
                    localScale.y *= -1f;
                }

                // Update the scale in case we preformed a flip
                transform.localScale = localScale;

                // Update bools regardless of what happens to the scales
                facingdown = true;
                facingLeft = true;
            }
        }

        // Repeat above described process for the other 3 quadrants
        else if (!IsDown(movement) && !IsLeft(movement))  // Quadrant 2
        {
            if (IsDown(movement) != facingdown || IsLeft(movement) != facingLeft)
            {
                Vector3 localScale = transform.localScale;
                if (localScale.x < 0)
                {
                    localScale.x *= -1f;
                }
                if (localScale.y < 0)
                {
                    localScale.y *= -1f;
                }

                transform.localScale = localScale;
                facingdown = false;
                facingLeft = false;
            }
        }
        else if (!IsDown(movement) && IsLeft(movement))  // Quadrant 3
        {
            if (IsDown(movement) != facingdown || IsLeft(movement) != facingLeft)
            {
                Vector3 localScale = transform.localScale;
                if (localScale.x < 0)
                {
                    localScale.x *= -1f;
                }
                if (localScale.y > 0)
                {
                    localScale.y *= -1f;
                }

                transform.localScale = localScale;
                facingdown = false;
                facingLeft = true;
            }
        }
        else if (IsDown(movement) && !IsLeft(movement))  // Quadrant 1
        {
            if (IsDown(movement) != facingdown || IsLeft(movement) != facingLeft)
            {
                Vector3 localScale = transform.localScale;
                if (localScale.x < 0)
                {
                    localScale.x *= -1f;
                }
                if (localScale.y < 0)
                {
                    localScale.y *= -1f;
                }

                transform.localScale = localScale;
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

}
