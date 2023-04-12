using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    public Transform player;  // To react to player movement
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
    private bool facingLeft = false;
    private bool facingdown = false;
    private bool canFlip = false;  // Keep charge of flipping once a frame
    public SpriteRenderer sprite_render;  // The enemy's sprite render

    private bool waitDone = false;  // Bool for if the bird wait
    private bool pathFollowing = false;  // Bool to see if following path

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
        Flip(direction); // Get correct position initially

    }

    IEnumerator wait()
    {
        sprite_render.enabled = false;
        yield return new WaitForSeconds(waitSeconds);  // Wait before moving
        waitDone = true;  // Update bool so update and fixed update can run
        sprite_render.enabled = true;
        // Get closer to the center of the collider instead of the sprite image
        Vector2 spriteCenter = new Vector2(player.position.x - 0.1f, player.position.y - 0.2f);
        // Initially request A* to attack the player
        PathRequest.RequestPath(transform.position, spriteCenter, OnPathFound);
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitDone)
        {
            return;
        }
        // Rotate by caluclated angle to face player
        if (!pathFollowing)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            direction.Normalize();
            movement = direction;
            rotation_angle = angle;
            Flip(movement);  // Flip the image to match the direction rotated
        }

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
            // Get closer to the center of the collider instead of the sprite image
            Vector2 spriteCenter = new Vector2(player.position.x - 0.1f, player.position.y - 0.2f);
            // Get an optimal path of attack
            PathRequest.RequestPath(transform.position, spriteCenter, OnPathFound);
            playerPos = player.position;  // Update player's current position
        }
    }

    private void LateUpdate()
    {
        // Called once a frame to allow the bird to flip and avoid glitches
        canFlip = true;
        print("update" + this.name);
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
        Collider2D collision = GetComponent<Collider2D>();
        if (path.Length > 0)  // Only move if we have somewhere to move
        {
            Vector2 currentWaypoint = path[0];  // Get the waypoint to move to
            while (true)
            {
                pathFollowing = true; // We are following a path right now
                // Make sure we have made the previous move
                if ((Vector2)transform.position == currentWaypoint)
                {
                    targetIndex++;  // Where we plan to go

                    // Stop if we can attack the player or we have traveresed all of our points
                    if (targetIndex >= path.Length || collision.gameObject.tag.Equals("Player"))
                    {
                        pathFollowing = false; // Done path following
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                Vector3 direction = currentWaypoint - (Vector2) transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                direction.Normalize();
                movement = direction;
                rotation_angle = angle;
                Flip(movement);  // Flip the image to match the direction rotated
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
        if (!canFlip)  // Only flip if you haven't this frame
        {
            return;
        }
        canFlip = false;  // Cannot flip for this frame now
        if (IsDown(direction) && IsLeft(direction))  // Quadrant 4
        {
            // Make sure we havent flipped in this direction already
            if (IsDown(direction) != facingdown || IsLeft(direction) != facingLeft)
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
        else if (!IsDown(direction) && !IsLeft(direction))  // Quadrant 2
        {
            if (IsDown(direction) != facingdown || IsLeft(direction) != facingLeft)
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
        else if (!IsDown(direction) && IsLeft(direction))  // Quadrant 3
        {
            if (IsDown(direction) != facingdown || IsLeft(direction) != facingLeft)
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
        else if (IsDown(direction) && !IsLeft(direction))  // Quadrant 1
        {
            if (IsDown(direction) != facingdown || IsLeft(direction) != facingLeft)
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
