using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    // For controlling enemy movement
    public Transform player;
    public float moveSpeed = 0.5f;
    private Rigidbody2D enemy_body;
    private Vector2 movement;
    public Collider2D guide;
    public MapGrid grid;

    // For animations
    private Animator anim;

    // Booleans for the flipping direction facing
    private bool facingLeft;

    // For Running Bresenham Algorithm
    // This will turn to true once the enemy spots Malakai
    private bool activated = false;
    // The higher the value, the smaller the chance of running the free sight
    public int freeSightChance = 100;
    // how far away the enemy can see
    public int sightRadius = 5;
    // 
    private bool nextStepIsOnTheGround = false;
    

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidbody and animator
        enemy_body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Freeze rotation and movement in y-axis
        enemy_body.freezeRotation = true;
        enemy_body.angularVelocity = 0f;
        enemy_body.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        activated |= Bresenham.determineActivation(freeSightChance, sightRadius, transform.position, player.position);
        if (activated) {
            // Rotate by calculated angle to face player
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            Flip(movement);  // Flip the image to match the direction to face

            // update animation
            anim.SetBool("walk", Mathf.Abs(direction[0]) > 0.1);
        }
    }

    private void FixedUpdate()
    {
        activated |=  Bresenham.determineActivation(freeSightChance, sightRadius, transform.position, player.position);
        if (activated && nextStepIsOnTheGround) {
            MoveEnemy(movement);  // Have enemy follow player
        } else if (activated) {
            // activated but the next step is no longer ground: do nothing
        } else {
            if (IsFacingRight()){
                // move right

                enemy_body.velocity = new Vector2(moveSpeed, 0f);
            } else {
                enemy_body.velocity = new Vector2(-moveSpeed, 0f);
                // move left
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { // when a collider exits another collider, this function runs
        nextStepIsOnTheGround = false;
        Debug.Log("The collider exited");
        if (activated){
            // in malakai hunting mode
            // kill the velocity because enemy is at the edge
            // enemy_body.velocity = new Vector2(0, 0f);
            // Debug.Log("Facing right?"); Debug.Log(IsFacingRight());
            // Node node = grid.NodeFromMapPoint((Vector2)transform.position + new Vector2(-guide.offset.x, guide.offset.y));
            // Debug.Log(transform.position);
            // Debug.Log(node);
            // Debug.Log("Next Step is ground?"); Debug.Log(NextStepIsStillGround());
            Debug.Log(nextStepIsOnTheGround);
            // nextStepIsOnTheGround = false;
        } else {
            // in patrol mode
            Node node = grid.NodeFromMapPoint((Vector2)transform.position + guide.offset);
            // if (IsFacingRight()){
            //     // move right
            //     Debug.Log("Facing right");
            //     node = grid.NodeFromMapPoint((Vector2)transform.position + guide.offset);
            // } else {
            //     Debug.Log("Facing left");
            //     node = grid.NodeFromMapPoint((Vector2)transform.position + new Vector2(-guide.offset.x, guide.offset.y));
            //     // move left
            // }
            Debug.Log(node);
            guide.offset = new Vector2(-guide.offset.x, guide.offset.y);
            facingLeft = !facingLeft;
            transform.localScale = new Vector2(-(Mathf.Sign(enemy_body.velocity.x)), transform.localScale.y);   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        nextStepIsOnTheGround = true;
        if (activated && !IsFacingRight()) Debug.Break();
        Debug.Log("The collider entered");
    }

    private bool NextStepIsStillGround()
    {
        if (IsFacingRight()) {
            // if the next point is free space (i.e. walkable), it is not ground
            return !grid.NodeFromMapPoint((Vector2)transform.position + guide.offset).walkable;
        } else {
            // Node node = grid.NodeFromMapPoint((Vector2)transform.position + new Vector2(-guide.offset.x, guide.offset.y));
            // Debug.Log(transform.position);
            // Debug.Log(node);
            // Debug.Break();
            Debug.Log("QWERTYUIOPASDFGHJKLZXCVBNM<");
            return !grid.NodeFromMapPoint((Vector2)transform.position + new Vector2(guide.offset.x, guide.offset.y)).walkable;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position, 0.5f);
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    // Move enemy position based on where the player is
    private void MoveEnemy(Vector2 direction)
    {
        enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        // if (grid.NodeFromMapPoint((Vector2)transform.position + guide.offset).walkable){
        //     // if the next step is no longer on the platform, don't do anything

        // } else {
        //     enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        // }
        // Node node;
        // if (IsFacingRight()){
        //     // move right
        //     Debug.Log("Facing left");
        //     node = grid.NodeFromMapPoint((Vector2)transform.position + new Vector2(-guide.offset.x, guide.offset.y));
        //     if (node.walkable) // at the edge
        //     {
        //         // do not move
        //     } else {
        //         enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        //     }
        // } else {
        //     Debug.Log("Facing right");
        //     node = grid.NodeFromMapPoint((Vector2)transform.position + guide.offset);
        //     // move left
        //     if (node.walkable) // at the edge
        //     {
        //         // do not move
        //     } else {
        //         enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        //     }
        // }
    }

    // Flip the enemy character depending on which direction it is facing
    private void Flip(Vector2 direction)
    {
        if (facingLeft && direction[0] > 0f || !facingLeft && direction[0] <= 0f)
        {
            facingLeft = !facingLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            guide.offset = new Vector2(-guide.offset.x, guide.offset.y);
        }
    }
}
